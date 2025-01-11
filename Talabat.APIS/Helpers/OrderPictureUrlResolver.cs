using AutoMapper;
using Talabat.APIS.DTOs;
using Talabat.Core.Order_Aggregate;

namespace Talabat.APIS.Helpers
{
	public class OrderPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
	{
		private readonly IConfiguration _configuration;

		public OrderPictureUrlResolver(IConfiguration configuration)
		{
			_configuration = configuration; // Inject IConfiguration Service To get "ApiBaseUrl" from appsettings.json
		}

		public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
			=> (!string.IsNullOrEmpty(source.Product.PictureUrl)) ? $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}" : string.Empty;
	}
}
