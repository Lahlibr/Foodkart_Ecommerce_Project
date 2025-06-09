using Foodkart.DTOs.AddDto;

namespace Foodkart.DTOs.ViewDto
{
    public class OrderViewDto
    {
        public string TransactionId { get; set; }
        public decimal? TotalAmount { get; set; }

        public string? DeliveryAdrress { get; set; }

        public string? Phone { get; set; }
        public DateTime OrderDate { get; set; }

        public List<OrderItemDto> Items { get; set; }
    }
}
