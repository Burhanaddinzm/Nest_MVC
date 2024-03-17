using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest.Data.Contexts;

namespace Nest.Area.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(m => m.Category)
                .Include(m => m.Images).Include(m => m.Vendor).ToListAsync();

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || id <= 0) return BadRequest();

            var product = await _context.Products
                .Include(x => x.Category)
                .Include(x => x.Images)
                .Include(x => x.ProductSizes).ThenInclude(x => x.Size)
                .Include(x => x.Vendor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }
    }
}
