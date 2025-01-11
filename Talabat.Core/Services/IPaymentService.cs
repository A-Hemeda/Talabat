using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IPaymentService
    {
        // Create Or Update PaymentIntentId Based On Basket Amount
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId);
        Task<Order> UpdatePaymentIntentToSucceedOrFailed(string paymentIntentId, bool flag);
    }
}
