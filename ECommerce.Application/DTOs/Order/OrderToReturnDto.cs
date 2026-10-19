using ECommerce.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Order
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public AddressDto ShippingAddress { get; set; } = default!;
        public ICollection<OrderItemDto> Items { get; set; } = [];
        public string Status { get; set; } = default!;
        public string DeliveryMethod { get; set; } = default!;
        public decimal DeliveryMethodCost { get; set; } = default!;
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}
