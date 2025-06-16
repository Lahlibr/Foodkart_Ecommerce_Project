namespace Foodkart.DTOs.ViewDto
{
    public class WishlistViewDto
    {
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public decimal? RealPrice { get; set; }
        public decimal OfferPrice { get; set; }



    }
}
