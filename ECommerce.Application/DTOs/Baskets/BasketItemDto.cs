using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.Baskets
{
    public class BasketItemDto
    {
        [Required(ErrorMessage = "Product Id Required !")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Name Required !")]
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        [Range(1, int.MaxValue)]
        public decimal Price { get; set; }
        [Range(1, 50)]
        public int Quantity { get; set; }
    }
}