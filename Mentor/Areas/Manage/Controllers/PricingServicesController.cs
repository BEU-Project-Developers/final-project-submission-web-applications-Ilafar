using Mentor.DAL;
using Mentor.Models;
using Mentor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Mentor.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class PricingServicesController(MentorAppDbContext mentorAppDbContext) : Controller
    {
        public IActionResult Index()
        {
            var uniquePricings = mentorAppDbContext.PricingServices
      .Include(ps => ps.Pricing)
      .GroupBy(ps => ps.Pricing.Id)
      .Select(g => g.First()) 
      .ToList();

            return View(uniquePricings);

        }

        public IActionResult Create()
        {
            ViewBag.Pricings = new SelectList(mentorAppDbContext.Pricings.ToList(), "Id", "Name");
            ViewBag.Services = new SelectList(mentorAppDbContext.Services.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(PricingServiceCreateVM vm)
        {
            ViewBag.Pricings = new SelectList(mentorAppDbContext.Pricings.ToList(), "Id", "Name");
            ViewBag.Services = new SelectList(mentorAppDbContext.Services.ToList(), "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            foreach (var serviceId in vm.ServiceIds)
            {
                var ps = new PricingService
                {
                    PricingId = vm.PricingId,
                    ServiceId = serviceId
                };
                mentorAppDbContext.PricingServices.Add(ps);
            }

            mentorAppDbContext.SaveChanges();
            return RedirectToAction("Index", "pricingServices");
        }

        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var existingServiceIds = mentorAppDbContext.PricingServices
                .Where(ps => ps.PricingId == id.Value)
                .Select(ps => ps.ServiceId)
                .ToList();

            if (!existingServiceIds.Any())
            {
                return NotFound();
            }

            var vm = new PricingServiceCreateVM
            {
                PricingId = id.Value,
                ServiceIds = existingServiceIds
            };

            ViewBag.Pricings = new SelectList(mentorAppDbContext.Pricings.ToList(), "Id", "Name", vm.PricingId);
            ViewBag.Services = new SelectList(mentorAppDbContext.Services.ToList(), "Id", "Name");

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(PricingServiceCreateVM vm)
        {
            ViewBag.Pricings = new SelectList(mentorAppDbContext.Pricings.ToList(), "Id", "Name", vm.PricingId);
            ViewBag.Services = new SelectList(mentorAppDbContext.Services.ToList(), "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // Əvvəl bütün köhnə əlaqələri sil
            var oldPricingServices = mentorAppDbContext.PricingServices
                .Where(ps => ps.PricingId == vm.PricingId)
                .ToList();

            mentorAppDbContext.PricingServices.RemoveRange(oldPricingServices);
            mentorAppDbContext.SaveChanges();

            // Sonra yenilərini əlavə et
            foreach (var serviceId in vm.ServiceIds)
            {
                mentorAppDbContext.PricingServices.Add(new PricingService
                {
                    PricingId = vm.PricingId,
                    ServiceId = serviceId
                });
            }

            mentorAppDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var pService = mentorAppDbContext.PricingServices.FirstOrDefault(t => t.PricingId == id);
            if (pService == null)
            {
                return NotFound();
            }


            mentorAppDbContext.PricingServices.Remove(pService);
            mentorAppDbContext.SaveChanges();

            return RedirectToAction("Index", "pricing");
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Əlavə olaraq Pricing və Service-lərlə birlikdə datanı gətir
            var pricing = mentorAppDbContext.Pricings.FirstOrDefault(p => p.Id == id);
            if (pricing == null)
                return NotFound();

            var serviceNames = mentorAppDbContext.PricingServices
                .Where(ps => ps.PricingId == id)
                .Include(ps => ps.Service)
                .Select(ps => ps.Service.Name)
                .ToList();

            var vm = new PricingServiceDetailVM
            {
                PricingId = pricing.Id,
                PricingName = pricing.Name,
                ServiceNames = serviceNames
            };

            return View(vm);
        }

    }
}
