    using Microsoft.AspNetCore.Mvc;

namespace Construction.Areas.Manage.Controllers;

[Area("Manage")]
public class PageController : Controller
{
    public IActionResult Forms()
    {
        return View();
    }
    public IActionResult Tables()
    {
        return View();
    }
}