using Mentor.DAL;
using Mentor.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mentor.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class PricingController(MentorAppDbContext mentorAppDbContext) : Controller
    {
        
        public IActionResult Index()
        {
            var pricings = mentorAppDbContext.Pricings.ToList();
            return View(pricings);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Pricing pricing)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var anyPricing = mentorAppDbContext.Pricings.FirstOrDefault(s => s.Name.ToUpper() == pricing.Name.ToUpper());
            if (anyPricing is not null)
            {
                ModelState.AddModelError(nameof(pricing.Name), "There is a name like that");
                return View();
            }
            mentorAppDbContext.Pricings.Add(pricing);
            mentorAppDbContext.SaveChanges();
            return RedirectToAction("Index", "Pricing");
        }

        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var pricingToEdit = mentorAppDbContext.Pricings.FirstOrDefault(t => t.Id == id);
            if (pricingToEdit == null)
            {
                return NotFound();
            }

            return View(pricingToEdit);
        }
        [HttpPost]
        public IActionResult Edit(Pricing pricing)
        {
            if (!ModelState.IsValid)
            {
                return View(pricing);
            }
            var anyPricing = mentorAppDbContext.Pricings.FirstOrDefault(s => s.Name.ToUpper() == pricing.Name.ToUpper());
            if (anyPricing is not null)
            {
                ModelState.AddModelError(nameof(pricing.Name), "There is a name like that");
                return View();
            }

            if (pricing == null)
            {
                return NotFound();
            }

            var existingPricing = mentorAppDbContext.Pricings.FirstOrDefault(t => t.Id == pricing.Id);
            if (existingPricing == null)
            {
                return NotFound();
            }
            existingPricing.Name = pricing.Name;
            existingPricing.Price = pricing.Price;
            existingPricing.IsAdvanced = pricing.IsAdvanced;
            existingPricing.IsFeatured = pricing.IsFeatured;
            mentorAppDbContext.SaveChanges();
            return RedirectToAction("Index", "pricing");
        }

        public IActionResult Delete(int id)
        {
            var pricing = mentorAppDbContext.Pricings.FirstOrDefault(t => t.Id == id);
            if (pricing == null)
            {
                return NotFound();
            }


            mentorAppDbContext.Pricings.Remove(pricing);
            mentorAppDbContext.SaveChanges();

            return RedirectToAction("Index", "pricing");
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ePricing = mentorAppDbContext.Pricings.FirstOrDefault(t => t.Id == id);
            if (ePricing is null)
                return NotFound();

            return View(ePricing);
        }

    }
}
