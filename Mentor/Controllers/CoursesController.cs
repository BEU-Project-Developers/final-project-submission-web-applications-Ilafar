using Mentor.DAL;
using Mentor.Models;
using Mentor.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mentor.Controllers
{
    public class CoursesController(MentorAppDbContext mentorAppDbContext,
        UserManager<AppUser> userManager) : Controller
    {
        public IActionResult Index()
        {
            try
            {
                var courses = mentorAppDbContext.Courses.Include(c => c.Trainer).ToList();
                ViewData["ActivePage"] = "Courses";
                return View(courses);
            }
            catch
            {
                return Problem();
            }
        }

        public IActionResult Detail(int? id)
        {
            if (id is null)
                return NotFound();

            try
            {
                var eCourse = mentorAppDbContext.Courses
                    .Include(c => c.Trainer)
                    .Include(c => c.CourseComments)
                    .ThenInclude(c => c.AppUser)
                    .FirstOrDefault(c => c.Id == id);

                if (eCourse == null)
                    return NotFound();

                CourseDetailVm courseDetailVm = new CourseDetailVm()
                {
                    Course = eCourse,
                    TotalComments = mentorAppDbContext.CourseComments.Count(bc => bc.CourseId == id),
                };

                if (User.Identity.IsAuthenticated)
                {
                    var userId = userManager.GetUserId(User);
                    var hasCourse = mentorAppDbContext.UserCourses
                        .Any(uc => uc.UserId == userId && uc.CourseId == id && uc.IsActive);

                    courseDetailVm.IsCourseBought = hasCourse;
                }

                return View(courseDetailVm);
            }
            catch
            {
                return Problem();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        public IActionResult AddComment([Bind(Prefix = "CourseComment")] CourseComment comment)
        {
            if (comment == null)
                return BadRequest("Comment data is missing.");

            try
            {
                var course = mentorAppDbContext.Courses
                    .Include(c => c.CourseComments)
                    .ThenInclude(c => c.AppUser)
                    .Include(c => c.Trainer)
                    .FirstOrDefault(c => c.Id == comment.CourseId);

                if (course == null)
                    return NotFound();

                var eUser = userManager.FindByNameAsync(User.Identity.Name).Result;
                if (eUser == null)
                    return RedirectToAction("Login", "GetSarted", new { returnUrl = Url.Action("Detail", "Courses", new { id = comment.CourseId }) });

                comment.AppUserId = eUser.Id;
                mentorAppDbContext.CourseComments.Add(comment);
                mentorAppDbContext.SaveChanges();

                return RedirectToAction("Detail", new { id = comment.CourseId });
            }
            catch
            {
                return Problem();
            }
        }

        public IActionResult DeleteComment(int? id)
        {
            if (id is null)
                return NotFound();

            try
            {
                var user = userManager.GetUserAsync(User).Result;
                var dComment = mentorAppDbContext.CourseComments.Include(bc => bc.AppUser).FirstOrDefault(bc => bc.Id == id);
                if (dComment == null)
                    return NotFound();

                if (dComment.AppUserId != user.Id)
                    return Content("You can not delete this comment");

                mentorAppDbContext.CourseComments.Remove(dComment);
                mentorAppDbContext.SaveChanges();

                return RedirectToAction("Detail", "Courses", new { id = dComment.CourseId });
            }
            catch
            {
                return Problem();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BuyCourse(int id)
        {
            try
            {
                var userId = userManager.GetUserId(User);

                var existing = await mentorAppDbContext.UserCourses
                    .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == id);

                var course = await mentorAppDbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
                if (course == null)
                    return NotFound();

                if (existing == null)
                {
                    var userCourse = new UserCourse
                    {
                        UserId = userId,
                        CourseId = id,
                        IsActive = true
                    };
                    course.BuyCount = course.BuyCount + 1;

                    mentorAppDbContext.UserCourses.Add(userCourse);
                    await mentorAppDbContext.SaveChangesAsync();
                }

                return RedirectToAction("Detail", "Courses", new { id });
            }
            catch
            {
                return Problem();
            }
        }
    }
}
