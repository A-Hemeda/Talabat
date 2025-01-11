namespace Talabat.APIS.Extension_Methods
{
	public static class SwaggerExtension
	{
		public static WebApplication UseSwaggerMiddlewares(this WebApplication app)
		{
			app.UseSwagger();
			app.UseSwaggerUI();
			return app;
		}
	}
}
