using System.Net;
using System.Text.Json;
using Talabat.APIS.Errors;

namespace Talabat.APIS.Middlewares
{
	// This middleware will handle all unhandled exceptions in the application, and return a structured error response
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next; // To refer to the next component in the request processing pipeline
		private readonly ILogger<ExceptionMiddleware> _logger; // To log the exception details
		private readonly IHostEnvironment _environment; // To customize the error response based on the environment

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
		{
			_next = next;
			_logger = logger;
			_environment = environment;
		}

		// Handel server error
		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				// If no error had occurred, request will passed to next middleware
				await _next.Invoke(httpContext);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
		{
			// Custom error response

			httpContext.Response.ContentType = "application/json";
			httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			var response = _environment.IsDevelopment()
				 // Development 
				 ? new ExceptionResponse(httpContext.Response.StatusCode, ex.Message, ex.StackTrace.ToString())
				 // Production
				 : new ExceptionResponse(httpContext.Response.StatusCode);

			var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

			var jsonResponse = JsonSerializer.Serialize(response, options);

			return httpContext.Response.WriteAsync(jsonResponse);
		}
	}
}
