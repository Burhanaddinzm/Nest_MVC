using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest.Data.Contexts;
using Nest.ViewModels;

namespace Nest.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        readonly AppDbContext _context;

        public ProductViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _context.Products
               .Include(m => m.Category)
               .Include(m => m.Vendor)
               .Include(m => m.Images)
               .OrderByDescending(m => m.Id)
               .Take(20)
               .ToListAsync();

            return View(products);
        }
    }
}
