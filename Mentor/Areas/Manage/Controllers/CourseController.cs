using Mentor.DAL;
using Mentor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Mentor.Areas.Manage.Controllers
{

    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly MentorAppDbContext mentorAppDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CourseController(MentorAppDbContext _mentorAppDbContext, IWebHostEnvironment webHostEnvironment)
        {
            mentorAppDbContext = _mentorAppDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var courses = mentorAppDbContext.Courses.ToList();
            return View(courses);
        }

        public IActionResult Create()
        {
            ViewBag.Trainers = new SelectList(mentorAppDbContext.Trainers.ToList(), "Id", "FullName");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Course course)
        {
            ViewBag.Trainers = new SelectList(mentorAppDbContext.Trainers.ToList(), "Id", "FullName");

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (course.Photo == null)
            {
                ModelState.AddModelError(nameof(course.Photo), "You must add a photo for course");
                return View();
            }

            if (!course.Photo.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError(nameof(course.Photo), "File must be an image");
                return View();
            }

            string fileExtension = Path.GetExtension(course.Photo.FileName);
            string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", uniqueFileName);

            using (FileStream stream = new FileStream(uploadPath, FileMode.Create))
            {
                course.Photo.CopyTo(stream);
            }

            course.Image = uniqueFileName;

            mentorAppDbContext.Courses.Add(course);
            mentorAppDbContext.SaveChanges();

            return RedirectToAction("Index", "course");
        }

        public IActionResult Edit(int? id)
        {
            ViewBag.Trainers = new SelectList(mentorAppDbContext.Trainers.ToList(), "Id", "FullName");

            if (!id.HasValue)
            {
                return NotFound();
            }

            var courseToEdit = mentorAppDbContext.Courses.Include(c=>c.Trainer).FirstOrDefault(t => t.Id == id);
            if (courseToEdit == null)
            {
                return NotFound();
            }

            return View(courseToEdit);
        }
        [HttpPost]
        public IActionResult Edit(Course course)
        {
            ViewBag.Trainers = new SelectList(mentorAppDbContext.Trainers.ToList(), "Id", "FullName");

            if (!ModelState.IsValid)
            {
                return View(course);
            }

            if (course == null)
            {
                return NotFound();
            }

            var existingcourse = mentorAppDbContext.Courses.FirstOrDefault(t => t.Id == course.Id);
            if (existingcourse == null)
            {
                return NotFound();
            }

            string previousImage = existingcourse.Image;

            if (course.Photo != null)
            {
                string extension = Path.GetExtension(course.Photo.FileName);
                string imageName = $"{Guid.NewGuid()}{extension}";
                string savePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", imageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    course.Photo.CopyTo(stream);
                }

                existingcourse.Image = imageName;

                string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", previousImage);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            existingcourse.Title = course.Title;
            existingcourse.Description = course.Description;
            existingcourse.Category = course.Category;
            existingcourse.Price = course.Price;
            existingcourse.TrainerId = course.TrainerId;
            existingcourse.Seats = course.Seats;
            existingcourse.StartTime = course.StartTime;
            existingcourse.EndTime = course.EndTime;


            mentorAppDbContext.SaveChanges();

            return RedirectToAction("Index", "Course");
        }

        public IActionResult Delete(int id)
        {
            var course = mentorAppDbContext.Courses.Include(c=>c.Trainer).FirstOrDefault(t => t.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", course.Image);

            if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            mentorAppDbContext.Courses.Remove(course);
            mentorAppDbContext.SaveChanges();

            return RedirectToAction("Index", "Course");
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var eCourse = mentorAppDbContext.Courses.Include(c=>c.Trainer).FirstOrDefault(t => t.Id == id);
            if (eCourse is null)
                return NotFound();

            return View(eCourse);
        }
    }
}
