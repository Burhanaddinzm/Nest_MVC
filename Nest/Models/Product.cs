using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nest.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Range(0.0, 5.0)]
        public double? Rating { get; set; }
        public decimal SellPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        [NotMapped]
        public ICollection<IFormFile>? Files { get; set; }
        [NotMapped]
        public IFormFile MainFile { get; set; }
        [NotMapped]
        public IFormFile HoverFile { get; set; }
        public int VendorId { get; set; }
        public int CategoryId { get; set; }
        public Vendor Vendor { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public List<ProductImage>? Images { get; set; } = null!;
        public ICollection<ProductSize>? ProductSizes { get; set; }
        public Product()
        {
            ProductSizes = new HashSet<ProductSize>();
        }
    }
}
