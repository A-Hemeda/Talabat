using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Talabat.APIS.Errors;
using Talabat.APIS.Profiles;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Services;

namespace Talabat.APIS.Extension_Methods
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add GenericRepository Service
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Add AutoMapper Service
            services.AddAutoMapper(typeof(MappingProfiles));

            // Add BasketRepository Service
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            // Handling Validation Error
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .SelectMany(e => e.Value.Errors)
                    .Select(em => em.ErrorMessage)
                    .ToArray();

                    var validationErrorResponse = new ValidationErrorResponse() { Errors = errors };

                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });

            // Add OrderService Service
            services.AddScoped(typeof(IOrderService), typeof(OrderService));

            // Add UnitOfWork Service
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add PaymentService Service
            services.AddScoped<IPaymentService, PaymentService>();

            // Add CORS Policy Service
            services.AddCors(optionsPolicy =>
            {
                optionsPolicy.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.WithOrigins(configuration["ClientBaseUrl"]);
                });
            });

            return services;
        }
    }
}
