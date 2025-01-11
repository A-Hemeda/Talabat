using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Order_Aggregate;
using ShippingAddress = Talabat.Core.Order_Aggregate.Address; // Alias  Name
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Specifications;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork; // Add This Service in ApplicationServicesExtension Class
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, ShippingAddress shippingAddress)
        {
            #region 1.Get Basket From Basket Repository
            var basket = await _basketRepository.GetBasketAsync(basketId);
            #endregion

            #region 2.Get Selected Items at Basket From Product Repository
            var orderItems = new List<OrderItem>();

            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }
            #endregion

            #region 3.Calculate SubTotal
            var subTotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);
            #endregion

            #region 4.Get Delivery Method From DeliveryMethod Repository
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            #endregion

            #region 5.Create Order
            var exOrder = await _unitOfWork.Repository<Order>()
                .GetEntityWithSpecificationAsync(new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId));

            if (exOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(exOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }

            var order = new Order(buyerEmail, OrderStatus.Pending, shippingAddress, deliveryMethod, orderItems, subTotal, basket.PaymentIntentId);
            #endregion

            #region 6.Add Order Locally
            await _unitOfWork.Repository<Order>().AddAsync(order);
            #endregion

            #region 7.Save Order To Database
            var result = await _unitOfWork.CompleteAsync();

            return (result <= 0) ? null : order;
            #endregion
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orderSpecifications = new OrderSpecifications(buyerEmail);

            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecificationAsync(orderSpecifications);

            return orders;
        }

        public async Task<Order> GetOrderByIdAsync(string buyerEmail, int orderId)
        {
            var orderSpecifications = new OrderSpecifications(buyerEmail, orderId);

            var orders = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationAsync(orderSpecifications);

            return orders;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
            => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
    }
}
