using System.ComponentModel.DataAnnotations.Schema;

namespace Nest.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Icon { get; set; } = null!;
        [NotMapped]
        public IFormFile File { get; set; } = null!;
        public List<Product>? Products { get; set; }
    }
}
