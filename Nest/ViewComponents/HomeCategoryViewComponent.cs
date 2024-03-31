using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest.Data.Contexts;

namespace Nest.ViewComponents
{
    public class HomeCategoryViewComponent : ViewComponent
    {
        readonly AppDbContext _context;

        public HomeCategoryViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories.Include(x => x.Products).ToListAsync();

            return View(categories);
        }
    }
}
