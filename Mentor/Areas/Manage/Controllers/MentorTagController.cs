using Mentor.DAL;
using Mentor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mentor.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class MentorTagController(MentorAppDbContext mentorAppDbContext) : Controller
    {
       
        public IActionResult Index()
        {
            var tags = mentorAppDbContext.MentorTags.ToList();
            return View(tags);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(MentorTags tag)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var anyService = mentorAppDbContext.Services.FirstOrDefault(s => s.Name.ToUpper() == tag.Name.ToUpper());
            if (anyService is not null)
            {
                ModelState.AddModelError(nameof(tag.Name), "There is a name like that");
                return View();
            }
            mentorAppDbContext.MentorTags.Add(tag);
            mentorAppDbContext.SaveChanges();
            return RedirectToAction("Index", "MentorTag");
        }

        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var tagToEdit = mentorAppDbContext.MentorTags.FirstOrDefault(t => t.Id == id);
            if (tagToEdit == null)
            {
                return NotFound();
            }

            return View(tagToEdit);
        }
        [HttpPost]
        public IActionResult Edit(MentorTags tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }
            var anyTag = mentorAppDbContext.MentorTags.FirstOrDefault(s => s.Name.ToUpper() == tag.Name.ToUpper());
            if (anyTag is not null)
            {
                ModelState.AddModelError(nameof(tag.Name), "There is a name like that");
                return View();
            }

            var existingTag = mentorAppDbContext.MentorTags.FirstOrDefault(t => t.Id == tag.Id);
            if (existingTag == null)
            {
                ModelState.AddModelError(string.Empty, "There is not any tag");
                return View();
            }
            existingTag.Name = tag.Name;
            mentorAppDbContext.SaveChanges();
            return RedirectToAction("Index", "MentorTag");

        }

        public IActionResult Delete(int id)
        {
            var tag = mentorAppDbContext.MentorTags.FirstOrDefault(t => t.Id == id);
            if (tag == null)
            {
                return NotFound();
            }


            mentorAppDbContext.MentorTags.Remove(tag);
            mentorAppDbContext.SaveChanges();

            return RedirectToAction("Index", "MentorTag");
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var eTag = mentorAppDbContext.MentorTags.FirstOrDefault(t => t.Id == id);
            if (eTag is null)
                return NotFound();

            return View(eTag);
        }
    }
}
