using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest.Data.Contexts;

namespace Nest.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductSizeController : Controller
    {
        readonly AppDbContext _context;

        public ProductSizeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pSizes = await _context.ProductSize
                .Include(x => x.Product)
                .Include(x => x.Size)
                .AsNoTracking()
                .ToListAsync();

            return View(pSizes);
        }
    }
}
