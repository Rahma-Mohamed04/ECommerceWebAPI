using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{

    public class OrderController : ApiBaseController
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto, CancellationToken ct)
        {
            return ToActionResult(await orderService.CreateOrderAsync(orderDto, GetEmailFromToken(), ct));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrders(CancellationToken ct)
        {
            return ToActionResult(await orderService.GetAllOrdersForUserAsync(GetEmailFromToken(), ct));
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid id, CancellationToken ct)
        {
            return ToActionResult(await orderService.GetOrderByIdAndEmailForUserAsync(id, GetEmailFromToken(), ct));
        }

        [AllowAnonymous]
        [HttpGet("DeliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethods(CancellationToken ct)
        {
            return ToActionResult(await orderService.GetDeliveryMethodsAsync(ct));
        }

    }
}
