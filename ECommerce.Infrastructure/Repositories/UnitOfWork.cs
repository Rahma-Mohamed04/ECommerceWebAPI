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
    public class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> repositories = new();
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var typeName = typeof(TEntity).Name; 
            if (repositories.TryGetValue(typeName, out object? value))
                return (IGenericRepository<TEntity, TKey>)value;
            else
            {
                var NewRepo = new GenericRepository<TEntity, TKey>(dbContext);
                repositories[typeName] = NewRepo;
                return NewRepo;
            }

        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await dbContext.SaveChangesAsync(ct);
        }
    }
}
