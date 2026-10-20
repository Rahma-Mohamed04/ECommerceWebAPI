using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface ICacheService
    {
        Task<string> GetDataAsync(string cacheKey, CancellationToken ct = default);

        Task SetDataAsync(string cacheKey, object cacheValue, TimeSpan? timeToLive = default, CancellationToken ct = default);
    }
}
