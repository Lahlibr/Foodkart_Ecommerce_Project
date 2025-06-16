namespace Foodkart.DTOs.ViewDto
{
    public class CartSummaryDto
    {
        public List<CartItemViewDto> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
        public decimal TotalPrice => Items?.Sum(i => i.TotalPrice) ?? 0;
    }
}
