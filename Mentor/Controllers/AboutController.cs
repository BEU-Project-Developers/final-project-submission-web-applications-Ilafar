using Mentor.DAL;
using Mentor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mentor.Controllers
{
    public class AboutController(MentorAppDbContext  mentorAppDbContext) : Controller
    {
        public IActionResult Index()
        {
            HomeVm vm = new HomeVm()
            {
                Courses = mentorAppDbContext.Courses.Include(c => c.Trainer).ToList(),
                Trainers = mentorAppDbContext.Trainers.ToList(),
                Users = mentorAppDbContext.AppUsers.ToList(),
                CourseComments = mentorAppDbContext.CourseComments.ToList()
            };
            ViewData["ActivePage"] = "About";
            return View(vm);
        }
    }
}
