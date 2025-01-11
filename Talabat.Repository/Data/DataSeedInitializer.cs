using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Order_Aggregate;

namespace Talabat.Repository.Data
{
	public static class DataSeedInitializer
	{
		public static async Task SeedDataAsync(API1DbContext dbContext)
		{
			#region Product Brands
			if (!dbContext.ProductBrands.Any()) // Check if No Data in Database
			{
				// 1-Serialize Data (Covert Data in File To a String)
				var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/Brands.json");

				// 2-Deserialize Data (Convert The String Data To List Of Specific Object)
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

				if (brands?.Count > 0) // Check brands is not null & more than 0
				{
					// 3-Add Data In Database
					foreach (var brand in brands)
						await dbContext.Set<ProductBrand>().AddAsync(brand);

					// 4- SaveChanges
					await dbContext.SaveChangesAsync();
				}
			}
			#endregion

			#region Product Types
			if (!dbContext.ProductTypes.Any()) // Check if No Data in Database
			{
				var typesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/Types.json");

				var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

				if (types?.Count > 0) // Check types is not null & more than 0
				{
					foreach (var type in types)
						await dbContext.Set<ProductType>().AddAsync(type);

					await dbContext.SaveChangesAsync();
				}
			}
			#endregion

			#region Products
			if (!dbContext.Products.Any()) // Check if No Data in Database
			{
				var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/Products.json");

				var products = JsonSerializer.Deserialize<List<Product>>(productsData);

				if (products?.Count > 0) // Check products is not null & more than 0
				{
					foreach (var product in products)
						await dbContext.Set<Product>().AddAsync(product);

					await dbContext.SaveChangesAsync();
				}
			}
			#endregion

			#region Delivery Methods
			if (!dbContext.DeliveryMethod.Any()) // Check if No Data in Database
			{
				// Serialize
				var deliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/Delivery.json");

				// Deserialize
				var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

				if (deliveryMethods?.Count > 0) // Check deliveryMethods is not null & more than 0
				{
					foreach (var deliveryMethod in deliveryMethods)
						await dbContext.Set<DeliveryMethod>().AddAsync(deliveryMethod);

					await dbContext.SaveChangesAsync();
				}
			}
			#endregion
		}
	}
}
