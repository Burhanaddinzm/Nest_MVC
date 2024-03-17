using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Nest.Data.Contexts;
using Nest.Extensions;
using Nest.Models;
using System.Drawing;

namespace Nest.Area.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(m => m.Category)
                .Include(m => m.Images).Include(m => m.Vendor).ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Vendor = await _context.Vendors.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid) return View(product);

            product.Images = new List<ProductImage>();
            var additionalProductImage = new ProductImage();

            if (product.Files != null)
            {
                foreach (var file in product.Files)
                {
                    var uniqueFileName = await file.SaveFileAsync(_env.WebRootPath, "client", "assets", "imgs/products");

                    additionalProductImage.Url = uniqueFileName;
                    additionalProductImage.IsMain = false;
                    additionalProductImage.IsHover = false;

                }
            }

            var mainFileName = await product.MainFile.SaveFileAsync(_env.WebRootPath, "client", "assets", "imgs/products");

            var mainProductImage = new ProductImage
            {
                Url = mainFileName,
                IsMain = true,
                IsHover = false
            };


            var hoverFileName = await product.MainFile.SaveFileAsync(_env.WebRootPath, "client", "assets", "imgs/products");

            var hoverProductImage = new ProductImage
            {
                Url = hoverFileName,
                IsMain = false,
                IsHover = true
            };


            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            additionalProductImage.ProductId = product.Id;
            mainProductImage.ProductId = product.Id;
            hoverProductImage.ProductId = product.Id;

            await _context.ProductImages.AddRangeAsync(product.Images);
            await _context.SaveChangesAsync();

            return View();
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || id <= 0) return BadRequest();

            var product = await _context.Products
                .Include(x => x.Category)
                .Include(x => x.Images)
                .Include(x => x.Vendor)
                .Include(x => x.ProductSizes)
                .ThenInclude(x => x.Size)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }
    }
}
