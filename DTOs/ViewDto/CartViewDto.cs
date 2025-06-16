namespace Foodkart.DTOs.ViewDto
{
    public class CartViewDto
    {
        public List<CartItemViewDto> Items { get; set; }
        public decimal TotalPrice => Items?.Sum(i => i.TotalPrice) ?? 0;
        public decimal TotalAmount => Items.Sum(i => i.TotalPrice);

    }
}
