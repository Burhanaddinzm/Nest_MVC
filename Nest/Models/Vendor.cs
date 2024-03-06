using System.ComponentModel.DataAnnotations.Schema;

namespace Nest.Models
{
    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Adress { get; set; } = null!;
        public string ContactInfo { get; set; } = null!;
        public double? Rating { get; set; }
        public string Logo { get; set; } = null!;
        [NotMapped]
        public IFormFile File { get; set; } = null!;
        public List<Product>? Products { get; set; }
    }
}
