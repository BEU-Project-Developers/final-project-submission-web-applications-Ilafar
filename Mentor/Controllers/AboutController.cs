using Microsoft.AspNetCore.Mvc;

namespace Mentor.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "About";
            return View();
        }
    }
}
