using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest.Data.Contexts;
using Nest.ViewModels;
using Newtonsoft.Json;

namespace Nest.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        readonly AppDbContext _context;

        public CartViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BasketVM>? basketVM = GetBasket();
            List<BasketItemsVM>? basketItemsVM = new List<BasketItemsVM>();

            foreach (var item in basketVM)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                basketItemsVM.Add(new BasketItemsVM
                {
                    Count = item.Count,
                    Id = product.Id,
                    Name = product.Name,
                    Image = product.Images.FirstOrDefault(m => m.IsMain).Url,
                    Price = product.SellPrice,
                    ProductId = item.ProductId
                });
            }

            return View(basketItemsVM);
        }

        List<BasketVM>? GetBasket()
        {
            List<BasketVM> basketVMs;
            if (Request.Cookies["Basket"] != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["Basket"]);
            }
            else basketVMs = new List<BasketVM>();

            return basketVMs;
        }
    }
}
