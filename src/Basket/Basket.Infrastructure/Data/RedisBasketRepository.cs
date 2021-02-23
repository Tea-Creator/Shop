using Basket.Application.Repositories;
using Basket.Domain.Models;

using System.Text.Json;
using System.Threading.Tasks;

namespace Basket.Infrastructure.Data
{
    public class RedisBasketRepository : IBasketRepository
    {
        private readonly RedisConnection _db;

        public RedisBasketRepository(RedisConnection db)
        {
            _db = db;
        }

        public async Task<BasketCart> Get(string username)
        {
            var basket = await _db.Redis.StringGetAsync(username);

            if (basket.IsNullOrEmpty)
            {
                return null;
            }

            return JsonSerializer.Deserialize<BasketCart>(basket.ToString());
        }

        public async Task<BasketCart> Update(BasketCart cart)
        {
            var updated = await _db.Redis.StringSetAsync(cart.Username, JsonSerializer.Serialize(cart));

            if (!updated)
            {
                return null;
            }

            return await Get(cart.Username);
        }

        public async Task<bool> Delete(string username)
        {
            return await _db.Redis.KeyDeleteAsync(username);
        }
    }
}
