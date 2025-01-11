using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.DTOs;
using Talabat.APIS.Errors;
using Talabat.APIS.Helpers;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIS.Controllers
{
	public class ProductsController : ApiBaseController
	{

		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
		{

			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

        #region EndPoints

        #region GET: BaseUrl/api/Product
        [ProducesResponseType(typeof(IReadOnlyList<ProductDto>), StatusCodes.Status200OK)]
        [HttpGet]
		public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts([FromQuery] ProductSpecificationParams productParams)
		{
			var specification = new ProductWithBrandAndTypeSpecification(productParams);

			var products = await _unitOfWork.Repository<Product>().GetAllWithSpecificationAsync(specification);

			var mappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);

			var paginationResponse = new PaginationResponse<ProductDto>
				(productParams.PageNumber, productParams.PageSize, _unitOfWork.Repository<Product>().GetAllAsync().Result.Count, mappedProducts);

			return Ok(paginationResponse);
		}
        #endregion

        #region GET: BaseUrl/api/Product/{id}
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
		public async Task<ActionResult<ProductDto>> GetProduct(int id)
		{
			var specification = new ProductWithBrandAndTypeSpecification(id);
			var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecificationAsync(specification);

			if (product is null)
				return NotFound(new ErrorResponse(404));

			var mappedProduct = _mapper.Map<Product, ProductDto>(product);

			return Ok(mappedProduct);
		}
        #endregion

        #region GET: BaseUrl/api/Product/Types
        [ProducesResponseType(typeof(IReadOnlyList<ProductType>), StatusCodes.Status200OK)]
        [HttpGet("Types")]
		public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductsTypes(string? sort)
		{
			var specification = new ProductTypesSpecification(sort);

			var productTypes = await _unitOfWork.Repository<ProductType>().GetAllWithSpecificationAsync(specification);

			return Ok(productTypes);
		}
        #endregion

        #region GET: BaseUrl/api/Product/Brands
        [ProducesResponseType(typeof(IReadOnlyList<ProductBrand>), StatusCodes.Status200OK)]
        [HttpGet("Brands")]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(string? sort)
		{
			var specification = new ProductBrandsSpecification(sort);

			var productBrands = await _unitOfWork.Repository<ProductBrand>().GetAllWithSpecificationAsync(specification);

			return Ok(productBrands);
		}
		#endregion

		#endregion
	}
}
