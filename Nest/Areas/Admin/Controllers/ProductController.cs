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
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Vendor = await _context.Vendors.ToListAsync();
            //if (!ModelState.IsValid) return View(product);

            product.Images = new List<ProductImage>();

            if (product.Files != null)
            {
                foreach (var file in product.Files)
                {
                    if (!file.CheckFileSize(2))
                    {
                        ModelState.AddModelError("", "File size can't exceed 2 MB!");
                        return View(product);
                    }

                    if (!file.CheckFileType("image"))
                    {
                        ModelState.AddModelError("", "File type is invalid!");
                        return View(product);
                    }

                    var uniqueFileName = await file.SaveFileAsync(_env.WebRootPath, "client", "assets", "imgs/products");

                    product.Images.Add(new ProductImage
                    {
                        Url = uniqueFileName,
                        IsHover = false,
                        IsMain = false,
                        Product = new Product
                        {
                            Name = product.Name,
                            Description = product.Description,
                            SellPrice = product.SellPrice,
                            DiscountPrice = product.DiscountPrice,
                            Rating = product.Rating,
                            CategoryId = product.CategoryId,
                            VendorId = product.VendorId,
                        }
                    });
                }
            }

            if (!product.MainFile.CheckFileSize(2))
            {
                ModelState.AddModelError("", "File size can't exceed 2 MB!");
                return View(product);
            }

            if (!product.MainFile.CheckFileType("image"))
            {
                ModelState.AddModelError("", "File type is invalid!");
                return View(product);
            }

            var mainFileName = await product.MainFile.SaveFileAsync(_env.WebRootPath, "client", "assets", "imgs/products");

            product.Images.Add(new ProductImage
            {
                Url = mainFileName,
                IsHover = false,
                IsMain = true,
                Product = new Product
                {
                    Name = product.Name,
                    Description = product.Description,
                    SellPrice = product.SellPrice,
                    DiscountPrice = product.DiscountPrice,
                    Rating = product.Rating,
                    CategoryId = product.CategoryId,
                    VendorId = product.VendorId,
                }
            });

            if (!product.HoverFile.CheckFileSize(2))
            {
                ModelState.AddModelError("", "File size can't exceed 2 MB!");
                return View(product);
            }

            if (!product.HoverFile.CheckFileType("image"))
            {
                ModelState.AddModelError("", "File type is invalid!");
                return View(product);
            }

            var hoverFileName = await product.HoverFile.SaveFileAsync(_env.WebRootPath, "client", "assets", "imgs/products");

            product.Images.Add(new ProductImage
            {
                Url = hoverFileName,
                IsHover = true,
                IsMain = false,
                Product = new Product
                {
                    Name = product.Name,
                    Description = product.Description,
                    SellPrice = product.SellPrice,
                    DiscountPrice = product.DiscountPrice,
                    Rating = product.Rating,
                    CategoryId = product.CategoryId,
                    VendorId = product.VendorId,
                }
            });


            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
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
