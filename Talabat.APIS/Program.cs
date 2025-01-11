using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIS.Errors;
using Talabat.APIS.Extension_Methods;
using Talabat.APIS.Middlewares;
using Talabat.APIS.Profiles;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIS
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			#region Create Host
			var builder = WebApplication.CreateBuilder(args);
			#endregion

			#region DI Container
			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			#region Extension Methods
			// Configure Entity Framework Core
			builder.Services.AddConnectionStrings(builder.Configuration);

			// Configure Redis
			builder.Services.AddRedisConnectionString(builder.Configuration);

			// Configure the Application Services
			builder.Services.AddApplicationServices(builder.Configuration);

			// Configure Identity Services
			builder.Services.AddIdentityServices(builder.Configuration);
			#endregion

			#endregion

			#region Build Project
			var app = builder.Build();
			#endregion

			#region Update Database [Apply Migrations]
			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				// Update Main Database
				var dbContext = services.GetRequiredService<API1DbContext>();
				await dbContext.Database.MigrateAsync();

				// Update Identity Database
				var IdentityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();
				await IdentityDbContext.Database.MigrateAsync();

				// Seeding Data (Main Database)
				await DataSeedInitializer.SeedDataAsync(dbContext);

				// Seeding Data (Identity)
				var userManger = services.GetRequiredService<UserManager<AppUser>>();
				await IdentityDataSeedInitializer.SeedUserAsync(userManger);
			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "An Error Occurred During Appling The Migration");
			}
			#endregion

			#region Middlewares
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				// Exception handling middleware to determine if the request will passed to the next middleware
				app.UseMiddleware<ExceptionMiddleware>();

				// Extension Method
				app.UseSwaggerMiddlewares();
			}

			app.UseStatusCodePagesWithRedirects("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseCors("MyPolicy");

			app.UseAuthentication();

			app.UseAuthorization();

			app.MapControllers();
			#endregion

			app.Run();
		}
	}
}
