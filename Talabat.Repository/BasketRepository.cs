using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repositories;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            // Ask CLR for object that implement interface IConnectionMultiplexer
            // Add This Service in program => Inside Class [ConfigureRedisExtension]
            _database = redis.GetDatabase();
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);

            return (basket.IsNull) ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);

            var createdOrUpdated = await _database.StringSetAsync(basket.Id, jsonBasket, TimeSpan.FromDays(1));

            return (!createdOrUpdated) ? null : await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }
    }
}
