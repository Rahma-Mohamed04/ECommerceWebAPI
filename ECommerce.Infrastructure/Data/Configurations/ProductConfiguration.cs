using ECommerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(X => X.ProductBrand)
                   .WithMany()
                   .HasForeignKey(X => X.BrandId);

            builder.HasOne(X => X.ProductType)
                   .WithMany()
                   .HasForeignKey(X => X.TypeId);

            builder.Property(P => P.Name).HasMaxLength(100);
            builder.Property(P => P.Description).HasMaxLength(500);
            builder.Property(P => P.PictureUrl).HasMaxLength(200);
            builder.Property(P => P.Price).HasColumnType("decimal(18, 2)");
        }
    }
}
