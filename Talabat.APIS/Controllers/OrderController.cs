using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIS.DTOs;
using Talabat.APIS.Errors;
using Talabat.Core.Order_Aggregate;
using Talabat.Core.Services;

namespace Talabat.APIS.Controllers
{
	public class OrderController : ApiBaseController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;

		public OrderController(IOrderService orderService, IMapper mapper)
		{
			_orderService = orderService; // Add This Service in ApplicationServicesExtension Class
			_mapper = mapper;
		}

		#region EndPoints

		#region POST:BaseUrl/api/Order
		[HttpPost]
		[Authorize]
		[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDto orderDto)
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

			var mappedAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

			var order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, mappedAddress);

			return (order is null) ? BadRequest(new ErrorResponse(400, "There Is A Problem With Creating Order")) : Ok(order);
		}
		#endregion

		#region GET:BaseUrl/api/Order
		[HttpGet]
		[Authorize]
		[ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

			var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);

			if (orders is null)
				return BadRequest(new ErrorResponse(404, "There Is No Orders For This User.."));

			var mappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders);

			return Ok(mappedOrders);
		}
		#endregion

		#region GET:BaseUrl/api/Order/{id}
		[HttpGet("{id}")]
		[Authorize]
		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

			var order = await _orderService.GetOrderByIdAsync(buyerEmail, id);

			if (order is null)
				return BadRequest(new ErrorResponse(404, $"There Is No Order For This User With Id = {id}.."));

			var mappedOrder = _mapper.Map<Order, OrderToReturnDto>(order);

			return Ok(mappedOrder);
		}
		#endregion

		#region GET:BaseUrl/api/Order/DeliveryMethods
		[HttpGet("DeliveryMethods")]
        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), StatusCodes.Status200OK)]
        [Authorize]
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
		{
			var deliveryMethod = await _orderService.GetDeliveryMethodsAsync();
			return Ok(deliveryMethod);
		}
		#endregion

		#endregion
	}
}
