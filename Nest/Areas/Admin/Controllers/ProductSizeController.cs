using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest.Areas.Admin.ViewModels;
using Nest.Data.Contexts;
using Nest.Models;

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

        public async Task<IActionResult> Create()
        {
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductSizeVM pSize)
        {
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();

            if (!ModelState.IsValid) return View(pSize);

            var product = _context.Products.AsNoTracking().FirstOrDefault(x => x.Id == pSize.ProductId);
            var size = _context.Sizes.AsNoTracking().FirstOrDefault(x => x.Id == pSize.SizeId);

            if (await _context.ProductSize.AnyAsync(x => x.Product.Name == product.Name && x.Size.Name == size.Name))
            {
                ModelState.AddModelError("", "This productsize already exists!");
                return View(pSize);
            }

            await _context.ProductSize.AddAsync((ProductSize)pSize);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
