using StackExchange.Redis;

namespace Talabat.APIS.Extension_Methods
{
	public static class ConfigureRedisExtension
	{
		public static IServiceCollection AddRedisConnectionString(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IConnectionMultiplexer>(options =>
			{
				var connection = configuration.GetConnectionString("RedisConnection");
				return ConnectionMultiplexer.Connect(connection);
			});

			return services;
		}
	}
}
