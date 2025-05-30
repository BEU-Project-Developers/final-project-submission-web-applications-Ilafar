using Microsoft.AspNetCore.Mvc;

namespace Mentor.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Contact";
            return View();
        }
    }
}
