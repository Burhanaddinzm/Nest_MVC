namespace Nest.ViewModels
{
    public class BasketItemsVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string Image { get; set; } = null!;
        public int ProductId { get; set; }
    }
}
