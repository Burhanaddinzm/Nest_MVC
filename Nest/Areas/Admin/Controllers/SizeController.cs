using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest.Data.Contexts;
using Nest.Extensions;
using Nest.Models;

namespace Nest.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizeController : Controller
    {
        readonly AppDbContext _context;

        public SizeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sizes = await _context.Sizes.AsNoTracking().ToListAsync();
            return View(sizes);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Size size)
        {
            size.Name = size.Name.ToUpper().Trim();

            if (_context.Sizes.Any(x => x.Name.ToUpper() == size.Name))
            {
                ModelState.AddModelError("", "Size already exists");
                return View(size);
            }

            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Size? size = await _context.Sizes.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Size size)
        {
            size.Name = size.Name.Trim().ToUpper();
            if (_context.Sizes.Any(x => x.Id != size.Id && x.Name.ToUpper().Trim() == size.Name))
            {
                ModelState.AddModelError("", $"Size: {size.Name} already exists!");
                return View(size);
            }

            _context.Update(size);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Size? size = await _context.Sizes.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Size? size = _context.Sizes.Find(id);

            if (size == null)
            {
                return NotFound();
            }

            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
