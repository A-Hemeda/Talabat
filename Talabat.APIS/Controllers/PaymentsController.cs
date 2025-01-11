using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIS.DTOs;
using Talabat.APIS.Errors;
using Talabat.Core.Models;
using Talabat.Core.Services;

namespace Talabat.APIS.Controllers
{
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        //const string endpointSecret = "whsec_c0d719d800efd553a3bf678024c1f18ffe636a2c109e8e45eae5a7a224105a3c";

        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }
        #region Endpoints

        #region POST:BaseUrl/api/Payment/basketId
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket is null) return BadRequest(new ErrorResponse(400, "There Is a Problem With Your Basket"));

            var mappedBasket = _mapper.Map<CustomerBasket, CustomerBasketDto>(basket);

            return Ok(mappedBasket);
        }
        #endregion

        #region POST:BaseUrl/api/Payment/webhook
        //[HttpPost("webhook")]
        //public async Task<IActionResult> StripeWebHook()
        //{
        //    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        //    try
        //    {
        //        var stripeEvent = EventUtility.ConstructEvent(json,
        //            Request.Headers["Stripe-Signature"], endpointSecret);

        //        var PaymentIntent = stripeEvent.Data.Object as PaymentIntent;

        //        //Handle the event
        //        if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
        //        {
        //            await _paymentService.UpdatePaymentIntentToSucceedOrFailed(PaymentIntent.Id, false);
        //        }
        //        else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
        //        {
        //            await _paymentService.UpdatePaymentIntentToSucceedOrFailed(PaymentIntent.Id, true);
        //        }

        //        return Ok();
        //    }
        //    catch (StripeException e)
        //    {
        //        return BadRequest();
        //    }

        //}
        #endregion

        #endregion
    }
}
