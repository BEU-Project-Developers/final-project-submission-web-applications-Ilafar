using Microsoft.AspNetCore.Mvc;

namespace Mentor.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Users() => View();
        public IActionResult UserCreate() => View();
        public IActionResult UserEdit(int id) => View();
        public IActionResult UserDelete(int id) => View();

        public IActionResult Courses() => View();
        public IActionResult CourseCreate() => View();
        public IActionResult CourseEdit(int id) => View();
        public IActionResult CourseDelete(int id) => View();

        public IActionResult Trainers() => View();
        public IActionResult TrainerCreate() => View();
        public IActionResult TrainerEdit(int id) => View();
        public IActionResult TrainerDelete(int id) => View();
    }
}
