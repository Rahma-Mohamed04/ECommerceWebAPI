using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Specification;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Order;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto order, string email, CancellationToken ct = default)
        {
            var basket = await basketRepository.GetBasketAsync(order.BasketId);
            if (basket == null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Not Found", "Basket Not Found"));

            var orderItems = new List<OrderItem>(basket.Items.Count);

            var productIds = basket.Items.Select(X => X.Id).ToHashSet();
            var products = (await unitOfWork.GetRepository<Product, int>()
            .GetAllAsync(new ProductWithIdSpecification(productIds), ct)).ToDictionary(x => x.Id);

            foreach (var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Result<OrderToReturnDto>.Fail(Error.NotFound("Not Found", "Product Not Found !"));

                orderItems.Add(new OrderItem()
                {
                    Price = product.Price,
                    Quantity = item.Quantity,
                    Product = new ProductItemOrdered()
                    {
                        PictureUrl = product.PictureUrl,
                        ProductId = product.Id,
                        ProductName = product.Name
                    }
                });
            }

            // Ship To Address
            var orderAddress = mapper.Map<OrderAddress>(order.ShipToAddress);

            // Delivery Method
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>()
                                                .GetByIdAsync(order.DeliveryMethodId);

            if (deliveryMethod == null) 
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Not Found", "Delivery Method Not Found"));

            // Sub Total
            var subTotal = orderItems.Sum(x => x.Quantity * x.Price);

            // Create Order
            var Order = new Order(email, orderAddress, orderItems, deliveryMethod, subTotal);
            unitOfWork.GetRepository<Order, Guid>().Add(Order);
            var result = await unitOfWork.SaveChangesAsync();

            if (result == 0)
                return Result<OrderToReturnDto>.Fail(Error.Failure("Failure", "Cannot Add Order"));
            else
            {
                await basketRepository.DeleteBasketAsync(order.BasketId, ct);
                return Result<OrderToReturnDto>.Ok(mapper.Map<OrderToReturnDto>(Order));
            }
            
        }

        public async Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForUserAsync(string email, CancellationToken ct = default)
        {
            var orders = await unitOfWork.GetRepository<Order, Guid>()
                                        .GetAllAsync(new OrderSpecification(email), ct); 
            if (orders.Any())
                return Result<IReadOnlyList<OrderToReturnDto>>.Ok(mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
            else
                return Result<IReadOnlyList<OrderToReturnDto>>.Fail(Error.NotFound("Not Found", "Orders Not Found"));
        }

        public async Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethodsAsync(CancellationToken ct = default)
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync(ct);
            if (deliveryMethods.Any())
                return Result<IReadOnlyList<DeliveryMethodDto>>.Ok(mapper.Map<IReadOnlyList<DeliveryMethodDto>>(deliveryMethods));
            else
                return Result<IReadOnlyList<DeliveryMethodDto>>.Fail(Error.NotFound("NotFound", "Delivery Method Not Found !"));
        }

        public async Task<Result<OrderToReturnDto>> GetOrderByIdAndEmailForUserAsync(Guid id, string email, CancellationToken ct = default)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>()
                                        .GetByIdAsync(new OrderSpecification(id, email), ct); 
            if (order != null)
                return Result<OrderToReturnDto>.Ok(mapper.Map<OrderToReturnDto>(order));
            else
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Not Found", "Order Not Found"));
        }
    }
}
