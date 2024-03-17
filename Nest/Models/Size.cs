namespace Nest.Models
{
    public class Size
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<ProductSize> ProductSizes { get; set; }
        public Size()
        {
            ProductSizes = new HashSet<ProductSize>();
        }
    }
}
