using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Specifications
{
    public class OrderWithPaymentIntentSpecifications : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecifications(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
