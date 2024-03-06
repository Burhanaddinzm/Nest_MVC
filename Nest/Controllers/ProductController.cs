using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest.Data.Contexts;
using Nest.ViewModels;

namespace Nest.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(m => m.Category)
                .Include(m => m.Vendor).Include(m => m.Images)
                .OrderByDescending(m => m.Id).Take(20)
                .ToListAsync();

            var categories = await _context.Categories.Include(m => m.Products).ToListAsync();

            ProductVM prodcutVM = new ProductVM()
            {
                Products = products,
                Categories = categories
            };

            return View(prodcutVM);
        }
    }
}
