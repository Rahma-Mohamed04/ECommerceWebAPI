using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto order, string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForUserAsync(string email, CancellationToken ct = default);
        Task<Result<OrderToReturnDto>> GetOrderByIdAndEmailForUserAsync(Guid id, string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethodsAsync(CancellationToken ct = default);
    }
}
