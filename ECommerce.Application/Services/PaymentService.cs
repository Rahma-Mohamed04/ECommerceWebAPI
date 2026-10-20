using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Baskets;
using ECommerce.Application.Specification;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Order;
using ECommerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPaymentGateway paymentGateway;
        private readonly IMapper mapper;
        private readonly PaymentGatewaySettings payment;

        public PaymentService(IBasketRepository basketRepository, 
                              IUnitOfWork unitOfWork, 
                              IPaymentGateway paymentGateway,
                              IOptions<PaymentGatewaySettings> options,
                              IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
            this.paymentGateway = paymentGateway;
            this.mapper = mapper;
            payment = options.Value;
        }
        public async Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default)
        {
            // 1) Get Basket 
            var basket = await basketRepository.GetBasketAsync(basketId, ct);

            if (basket == null)
                return Result<BasketDto>.Fail(Error.NotFound("NotFound", "Basket Not Found !"));
            if (basket.Items.Count == 0)
                return Result<BasketDto>.Fail(Error.Validation("Validation", "Basket Has No Items !"));

            // 2) Get Delivery Method Cost
            if (!basket.DeliveryMethodId.HasValue)
                return Result<BasketDto>.Fail(Error.NotFound("NotFound", "Delivery Method Not Found !"));
            
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod == null)
                return Result<BasketDto>.Fail(Error.NotFound("NotFound", "Delivery Method Not Found !"));

            basket.ShippingPrice = deliveryMethod.Price;

            // 3) Products Prices
            var productIds = basket.Items.Select(X => X.Id).ToHashSet();
            var products = (await unitOfWork.GetRepository<Product, int>()
                                            .GetAllAsync(new ProductWithIdSpecification(productIds), ct)).ToDictionary(x => x.Id);
            
            foreach (var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Result<BasketDto>.Fail(Error.NotFound("Not Found", "Product Not Found !"));

                item.Price = product.Price;
            }

            // 4) Total Amount
            var subTotal = basket.Items.Sum(I => I.Price * I.Quantity);
            var amount = (long)((subTotal + deliveryMethod.Price) * 100m);
            // Long As Cents => Payment Configuration

            // 5) PaymentIntent
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // Create
                var result = await paymentGateway.CreatePaymentIntentAsync(amount, payment.DefaultCurrency, ct);
                basket.PaymentIntentId = result.PaymentIntentId;
                basket.ClientSecret = result.ClientSecret;
            }
            else
            {
                var result = await paymentGateway.UpdatePaymentIntentAsync(amount, basket.PaymentIntentId, ct);
            }

            await basketRepository.CreateOrUpdateBasketAsync(basket, ct: ct);
            return Result<BasketDto>.Ok(mapper.Map<BasketDto>(basket)); 

        }
    }
}
