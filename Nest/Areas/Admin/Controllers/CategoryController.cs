using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest.Data.Contexts;
using Nest.Extensions;
using Nest.Models;
using IO = System.IO;

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
            if (!category.File.CheckFileType("image"))
            {
                ModelState.AddModelError("", "Invalid file type!");
                return View(category);
            }
            if (!category.File.CheckFileSize(2))
            {
                ModelState.AddModelError("", "File size too big!");
                return View(category);
            }

            string uniqueFileName = await category.File.SaveFileAsync(_env.WebRootPath, "client", "assets", "categoryIcons");

            Category newCategory = new Category
            {
                Name = category.Name,
                Icon = uniqueFileName
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Category category)
        {
            if (id != category.Id) return BadRequest();

            Category? existingCategory = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (existingCategory == null) return NotFound();

            if (category.File != null)
            {
                if (!category.File.CheckFileType("image"))
                {
                    ModelState.AddModelError("", "Invalid file type!");
                    return View(category);
                }
                if (!category.File.CheckFileSize(2))
                {
                    ModelState.AddModelError("", "File size too big!");
                    return View(category);
                }

                var path = Path.Combine(_env.WebRootPath, "client", "assets", "categoryIcons", existingCategory.Icon);

                if (IO.File.Exists(path))
                {
                    IO.File.Delete(path);
                }

                var uniqueFileName = await category.File.SaveFileAsync(_env.WebRootPath, "client", "assets", "categoryIcons");

                existingCategory.Icon = uniqueFileName;
                existingCategory.Name = category.Name;
                _context.Update(existingCategory);
            }
            else
            {
                category.Icon = existingCategory.Icon;
                _context.Categories.Update(category);
            }

            if (category.Name == null)
            {
                return RedirectToAction("Edit", id);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
