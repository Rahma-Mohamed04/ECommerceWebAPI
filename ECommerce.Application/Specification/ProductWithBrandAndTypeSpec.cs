using ECommerce.Application.Common;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Specification
{
    public class ProductWithBrandAndTypeSpec : BaseSpecification<Product, int>
    {
        public ProductWithBrandAndTypeSpec(ProductQueryParams queryParams) :
            base(p => (queryParams.BrandId == null || p.BrandId == queryParams.BrandId)
                  && (queryParams.TypeId == null || p.TypeId == queryParams.TypeId)
                  && (queryParams.Search == null || p.Name.ToLower().Contains(queryParams.Search.ToLower())))
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);

            switch (queryParams.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(P => P.Name);
                    break;

                case ProductSortingOptions.NameDesc:
                    AddOrderByDesc(P => P.Name);
                    break;

                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(P => P.Price);
                    break;

                case ProductSortingOptions.PriceDesc:
                    AddOrderByDesc(P => P.Price);
                    break;

                default:
                    AddOrderBy(P => P.Id);
                    break;
            };

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }
        public ProductWithBrandAndTypeSpec(int id) : base(p => p.Id == id)
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }
    }
}
