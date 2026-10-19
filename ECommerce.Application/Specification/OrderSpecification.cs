using ECommerce.Application.DTOs.Order;
using ECommerce.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Specification
{
    public class OrderSpecification : BaseSpecification<Order, Guid>
    {
        public OrderSpecification(string email) : base(x => x.BuyerEmail == email)
        {
            AddInclude(X => X.DeliveryMethod);
            AddInclude(X => X.Items);
            AddOrderByDesc(O => O.OrderDate);
        }

        public OrderSpecification(Guid id, string email) : base(x => x.BuyerEmail == email && x.Id == id)
        {
            AddInclude(X => X.DeliveryMethod);
            AddInclude(X => X.Items);
            AddOrderByDesc(O => O.OrderDate);
        }
    }
}
