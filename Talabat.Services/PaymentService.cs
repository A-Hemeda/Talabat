using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Product = Talabat.Core.Models.Product;
using Talabat.Core.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Specifications;

namespace Talabat.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            // Get Your SecretKey To Communicate With Strip And Put It In StripeConfiguration
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            // Get CustomerBasket To Create PaymentIntent Based On Amount of Basket
            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket is null) return null;

            // If basket not null get The DeliveryMethod.Cost And SubTotal Because the PaymentIntent Depends on the Amount of Basket
            #region Calculate Amount of Basket (SubTotal + ShippingPrice [DeliveryMethod.Cost])

            #region Calculate ShippingPrice
            var shippingPrice = 0M;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }
            #endregion

            #region Calculate SubToTal
            if (basket.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            var subToTal = basket.Items.Sum(item => item.Price * item.Quantity);
            #endregion

            #endregion

            #region Create Or Update PaymentIntentId
            var paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // Create
            {
                var paymentOptions = new PaymentIntentCreateOptions()
                {
                    Amount = ((long)subToTal * 100) + ((long)shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(paymentOptions);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update
            {
                var paymentOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = ((long)subToTal * 100) + ((long)shippingPrice * 100),
                };

                paymentIntent = await paymentIntentService.UpdateAsync(basket.PaymentIntentId, paymentOptions);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

            await _basketRepository.UpdateBasketAsync(basket);
            #endregion

            return basket;
        }

        public async Task<Order> UpdatePaymentIntentToSucceedOrFailed(string paymentIntentId, bool flag)
        {
            var specification = new OrderWithPaymentIntentSpecifications(paymentIntentId);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationAsync(specification);

            order.Status = flag ? OrderStatus.PaymentReceived : OrderStatus.PaymentFailed;

            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.CompleteAsync();

            return order;
        }
    }
}
