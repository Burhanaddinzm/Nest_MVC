﻿namespace Nest.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public double? Rating { get; set; }
        public decimal SellPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int VendorId { get; set; }
        public int CategoryId { get; set; }
        public Vendor Vendor { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public List<ProductImage> Images { get; set; } = null!;
    }
}
