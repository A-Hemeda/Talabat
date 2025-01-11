using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; } // Aggregation Relationship
        //public int DeliveryMethodId { get; set; } // FK
        public DeliveryMethod DeliveryMethod { get; set; } // Navigational property
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal SubTotal { get; set; } // PriceOfProduct * Quantity
        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;
        public string PaymentIntentId { get; set; }

        #region Constructors
        public Order() { }

        public Order(string buyerEmail, OrderStatus status, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            Status = status;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }
        #endregion
    }
}
