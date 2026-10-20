using AutoMapper;
using ECommerce.Application.DTOs.Order;
using ECommerce.Domain.Entities.Order;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Profiles
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly UrlSettings settings;

        public OrderItemPictureUrlResolver(IOptions<UrlSettings> settings)
        {
            this.settings = settings.Value;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            var baseUrl = settings.BaseUrl.TrimEnd('/');
            var path = source.Product.PictureUrl.TrimStart('/');
            return $"{baseUrl}/Files/{path}";
        }
    }

    public class UrlSettings
    {
        public string BaseUrl { get; set; }
    }
}
