﻿using System.ComponentModel.DataAnnotations;
using Foodkart.DTOs.Products;
using Microsoft.AspNetCore.Http;

namespace Foodkart.DTOs.ViewDto
{
    public class CategoryViewDto
    {
        public int CategoryId {  get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name must be under 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description can be up to 500 characters.")]
        public string? Description { get; set; }

        // If you're uploading the image
        [Required(ErrorMessage = "Image is required.")]
        public string ImageUrl { get; set; }
        public List<ProductViewDto> Products { get; set; }
    }
}
