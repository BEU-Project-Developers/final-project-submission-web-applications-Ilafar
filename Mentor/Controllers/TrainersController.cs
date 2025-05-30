using Mentor.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Mentor.Controllers
{
    public class TrainersController(MentorAppDbContext mentorAppDbContext) : Controller
    {
        public IActionResult Index()
        {
            var trainers = mentorAppDbContext.Trainers.ToList();
            ViewData["ActivePage"] = "Trainers";
            return View(trainers);
        }
    }
}
