﻿namespace Foodkart.DTOs.ViewDto
{
    public class CartItemViewDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public decimal? RealPrice { get; set; }
        public decimal OfferPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => OfferPrice * Quantity;
       
    }
}
