using Microsoft.AspNetCore.Mvc;

namespace Mentor.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Events";
            return View();
        }
    }
}
