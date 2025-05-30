using Mentor.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mentor.Controllers
{
    public class CoursesController(MentorAppDbContext mentorAppDbContext) : Controller
    {
        public IActionResult Index()
        {
            var courses = mentorAppDbContext.Courses.Include(c=>c.Trainer).ToList();
            ViewData["ActivePage"] = "Courses";
            return View(courses);
        }
        public IActionResult Detail(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var eCourse = mentorAppDbContext.Courses.Include(c => c.Trainer).FirstOrDefault(c=>c.Id==id);
            if (eCourse == null) 
                return NotFound();

            return View(eCourse);
        }
    }
}
