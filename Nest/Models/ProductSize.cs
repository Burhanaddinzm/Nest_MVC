using Nest.Areas.Admin.ViewModels;

namespace Nest.Models
{
    public class ProductSize
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int SizeId { get; set; }
        public Size Size { get; set; }
        public int Count { get; set; }

        public static explicit operator ProductSize(ProductSizeVM productSizeVM)
        {
            return new ProductSize
            {
                ProductId = productSizeVM.ProductId,
                SizeId = productSizeVM.SizeId,
                Count = productSizeVM.Count
            };
        }
    }
}
