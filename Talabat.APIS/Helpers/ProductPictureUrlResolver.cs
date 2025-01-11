using AutoMapper;
using AutoMapper.Execution;
using Talabat.APIS.DTOs;
using Talabat.Core.Models;

namespace Talabat.APIS.Helpers
{
	public class ProductPictureUrlResolver : IValueResolver<Product, ProductDto, string>
	{
		private readonly IConfiguration _configuration;

		public ProductPictureUrlResolver(IConfiguration configuration)
		{
			_configuration = configuration; // Inject IConfiguration Service To get "ApiBaseUrl" from appsettings.json
		}

		public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
			=> (!string.IsNullOrEmpty(source.PictureUrl)) ? $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}" : string.Empty;
	}
}
