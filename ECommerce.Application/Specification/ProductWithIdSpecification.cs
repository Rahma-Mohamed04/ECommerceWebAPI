using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Specification
{
    public class ProductWithIdSpecification : BaseSpecification<Product, int>
    {
        public ProductWithIdSpecification(HashSet<int> productIds) : base(P => productIds.Contains(P.Id))
        { 
        
        }
    }
}
