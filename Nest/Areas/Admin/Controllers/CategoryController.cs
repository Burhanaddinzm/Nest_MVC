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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0) { RedirectToAction("Index"); }

            var category = _context.Categories.Find(id);

            if (category == null) { RedirectToAction("Index"); }

            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Category category)
        {
            if (category.File == null)
            {
                category.Icon = _context.Categories.AsNoTracking().FirstOrDefault(m => m.Id == id).Icon;
                await Console.Out.WriteLineAsync(category.Icon);
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return View();
            }


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

            category.Icon = uniqueFileName;

            string path = Path.Combine(_env.WebRootPath, "client", "assets", "categoryIcons", uniqueFileName);

            FileStream fs = new FileStream(path, FileMode.Create);

            await category.File.CopyToAsync(fs);

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) { RedirectToAction("Index"); }

            var category = _context.Categories.Find(id);

            if (category == null) { RedirectToAction("Index"); }

            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            if (id == null || id == 0) { RedirectToAction("Index"); }

            var category = _context.Categories.Find(id);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
