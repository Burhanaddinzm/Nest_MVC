using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest.Data.Contexts;
using Nest.Models;

namespace Nest.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Include(m => m.Products).ToListAsync();

            return View(categories);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!category.File.ContentType.Contains("image"))
            {
                ModelState.AddModelError("", "Invalid file type!");
                return View(category);
            }
            if (category.File.Length / 1024 / 1024 > 1)
            {
                ModelState.AddModelError("", "File size too big!");
                return View(category);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + category.File.FileName;

            Category newCategory = new Category
            {
                Name = category.Name,
                Icon = uniqueFileName
            };

            string path = Path.Combine(_env.WebRootPath, "client", "assets", "categoryIcons", uniqueFileName);

            FileStream fs = new FileStream(path, FileMode.Create);

            await category.File.CopyToAsync(fs);

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            return View();
        }
    }
}
