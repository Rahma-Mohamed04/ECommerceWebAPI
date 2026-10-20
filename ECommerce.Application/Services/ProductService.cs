using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Specification;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken ct = default)
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(ct);
            var data = mapper.Map<IReadOnlyList<BrandDto>>(brands);
            return Result<IReadOnlyList<BrandDto>>.Ok(data);
        }

        public async Task<Result<PaginatedResult<ProductDto>>> GetAllProductsAsync(ProductQueryParams queryParams, CancellationToken ct = default)
        {
            var spec = new ProductWithBrandAndTypeSpec(queryParams);
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(spec, ct);
            var data = mapper.Map<IReadOnlyList<ProductDto>>(products);

            var countSpec = new ProductCountSpecification(queryParams);
            var count = await unitOfWork.GetRepository<Product, int>().CountAsync(countSpec);

            var result = new PaginatedResult<ProductDto>(queryParams.PageIndex, queryParams.PageSize, count, data);

            return Result<PaginatedResult<ProductDto>>.Ok(result);
        }

        public async Task<Result<IReadOnlyList<TypeDto>>> GetAllTypesAsync(CancellationToken ct = default)
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync(ct);
            var data = mapper.Map<IReadOnlyList<TypeDto>>(types);
            return Result<IReadOnlyList<TypeDto>>.Ok(data);
        }

        public async Task<Result<ProductDto>> GetProductByIdAsync(int id, CancellationToken ct = default)
        {
            var spec = new ProductWithBrandAndTypeSpec(id);
            var product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(spec, ct);
            if (product is null)
                return Result<ProductDto>.Fail(Error.NotFound("Product.NotFound", $"Product With Id {id} Not Found"));
            return Result<ProductDto>.Ok(mapper.Map<ProductDto>(product));
        }
    }
}
