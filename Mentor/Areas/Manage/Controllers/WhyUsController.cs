using Mentor.DAL;
using Mentor.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mentor.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class WhyUsController(MentorAppDbContext mentorAppDbContext) : Controller
    {
        public IActionResult Index()
        {
            var whyuses = mentorAppDbContext.WhyUs.ToList();
            return View(whyuses);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(WhyUs whyUs)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var anywhyUs = mentorAppDbContext.WhyUs.FirstOrDefault(s => s.Name.ToUpper() == whyUs.Name.ToUpper());
            if (anywhyUs is not null)
            {
                ModelState.AddModelError(nameof(whyUs.Name), "There is a cause like that");
                return View();
            }
            mentorAppDbContext.WhyUs.Add(whyUs);
            mentorAppDbContext.SaveChanges();
            return RedirectToAction("Index", "whyUs");
        }

        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var whyUsToEdit = mentorAppDbContext.WhyUs.FirstOrDefault(t => t.Id == id.Value);
            if (whyUsToEdit == null)
            {
                return NotFound();
            }

            return View(whyUsToEdit);
        }
        [HttpPost]
        public IActionResult Edit(WhyUs whyUs)
        {
            if (!ModelState.IsValid)
            {
                return View(whyUs);
            }
            var anywhyUs = mentorAppDbContext.WhyUs.FirstOrDefault(s => s.Name.ToUpper() == whyUs.Name.ToUpper());
            if (anywhyUs is not null)
            {
                ModelState.AddModelError(nameof(whyUs.Name), "There is a cause like that");
                return View();
            }

            if (whyUs == null)
            {
                return NotFound();
            }

            var existingwhyUs = mentorAppDbContext.WhyUs.FirstOrDefault(t => t.Id == whyUs.Id);
            if (existingwhyUs == null)
            {
                return NotFound();
            }
            existingwhyUs.Name = whyUs.Name;
            existingwhyUs.Description = whyUs.Description;
            existingwhyUs.Icon = whyUs.Icon;
            mentorAppDbContext.SaveChanges();
            return RedirectToAction("Index", "whyUs");

        }

        public IActionResult Delete(int id)
        {
            var whyUs = mentorAppDbContext.WhyUs.FirstOrDefault(t => t.Id == id);
            if (whyUs == null)
            {
                return NotFound();
            }


            mentorAppDbContext.WhyUs.Remove(whyUs);
            mentorAppDbContext.SaveChanges();

            return RedirectToAction("Index", "whyUs");
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ewhyUs = mentorAppDbContext.WhyUs.FirstOrDefault(t => t.Id == id);
            if (ewhyUs is null)
                return NotFound();

            return View(ewhyUs);
        }
    }
}
