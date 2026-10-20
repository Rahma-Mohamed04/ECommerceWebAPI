using ECommerce.Domain.Entities.Order;
using ECommerce.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
             builder.HasMany(O => O.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(X => X.SubTotal).HasColumnType("decimal(8,2)");

            builder.OwnsOne(O => O.ShippingAddress, address =>
            {
                address.Property(P => P.FirstName).HasMaxLength(50);
                address.Property(P => P.LastName).HasMaxLength(50);
                address.Property(P => P.City).HasMaxLength(50);
                address.Property(P => P.Street).HasMaxLength(50);
                address.Property(P => P.Country).HasMaxLength(50);
            });
            
            builder.Property(X => X.Status).HasConversion<string>().HasMaxLength(50);
        }
    }
}
