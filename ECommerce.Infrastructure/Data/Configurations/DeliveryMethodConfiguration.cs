using ECommerce.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(X => X.Price).HasColumnType("decimal(8,2)");
            builder.Property(X => X.ShortName).HasMaxLength(50);
            builder.Property(X => X.Description).HasMaxLength(100);
            builder.Property(X => X.DeliveryTime).HasColumnType("varchar").HasMaxLength(50);
        }
    }
}
