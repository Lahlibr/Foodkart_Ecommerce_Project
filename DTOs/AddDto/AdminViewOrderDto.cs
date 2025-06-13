namespace Foodkart.DTOs.AddDto
{
    public class AdminViewOrderDto
    {
        public int OrderId { get; set; }
        public string TransactionId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
