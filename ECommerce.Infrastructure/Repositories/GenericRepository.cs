using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, TKey>(StoreDbContext dbContext) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public void Add(TEntity entity)
        {
            dbContext.Set<TEntity>().Add(entity);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
        {
            return await dbContext.Set<TEntity>().ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default)
        {
            return await dbContext.Set<TEntity>().FindAsync(id, ct);
        }

        public void Remove(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
        }
    }
}
