using StackExchange.Redis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Infrastructure.Data
{
    public class RedisConnection
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisConnection(ConnectionMultiplexer redis)
        {
            _redis = redis;
            Redis = _redis.GetDatabase();
        }

        public IDatabase Redis { get; }
    }
}
