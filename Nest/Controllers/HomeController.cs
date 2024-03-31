using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Nest.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProductCategoryFilter(int? id)
        {
            return ViewComponent("Product", new { categoryId = id });
        }
    }
}
