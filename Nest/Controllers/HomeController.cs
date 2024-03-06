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
    }
}
