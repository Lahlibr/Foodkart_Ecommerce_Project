namespace Foodkart.DTOs.AddDto
{
    public class CreateOrderItemDto
    {
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductType { get; set; } = null;
        public int AddressId { get; set; }
        public int TransactionId { get; set; }
       
    }
}
