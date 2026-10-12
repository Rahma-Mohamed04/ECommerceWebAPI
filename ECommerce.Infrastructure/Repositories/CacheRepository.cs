using ECommerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _database;

        public CacheRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public async Task<string?> GetAsync(string cacheKey, CancellationToken ct = default)
        {
            var value = await _database.StringGetAsync(cacheKey);
            if (value.IsNullOrEmpty) return null;
            return value.ToString();
        }

        public async Task SetAsync(string cacheKey, string cacheValue, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
            await _database.StringSetAsync(cacheKey, cacheValue, timeToLive ?? TimeSpan.FromDays(2));
        }
    }
}
