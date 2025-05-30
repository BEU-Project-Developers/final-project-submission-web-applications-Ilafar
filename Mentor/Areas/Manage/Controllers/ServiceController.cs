using Mentor.DAL;
using Mentor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Mentor.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class ServiceController(MentorAppDbContext mentorAppDbContext) : Controller
    {
        public IActionResult Index()
        {
            var services = mentorAppDbContext.Services.ToList();
            return View(services);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Service service)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var anyService = mentorAppDbContext.Services.FirstOrDefault(s=>s.Name.ToUpper()==service.Name.ToUpper());
            if (anyService is not null)
            {
                ModelState.AddModelError(nameof(service.Name), "There is a name like that");
                return View();
            }
            mentorAppDbContext.Services.Add(service);
            mentorAppDbContext.SaveChanges();
            return RedirectToAction("Index", "Service");
        }

        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var serviceToEdit = mentorAppDbContext.Services.FirstOrDefault(t => t.Id == id.Value);
            if (serviceToEdit == null)
            {
                return NotFound();
            }

            return View(serviceToEdit);
        }
        [HttpPost]
        public IActionResult Edit(Service service)
        {
            if (!ModelState.IsValid)
            {
                return View(service);
            }
            var anyService = mentorAppDbContext.Services.FirstOrDefault(s => s.Name.ToUpper() == service.Name.ToUpper());
            if (anyService is not null)
            {
                ModelState.AddModelError(nameof(service.Name), "There is a name like that");
                return View();
            }

            if (service == null)
            {
                return NotFound();
            }

            var existingService = mentorAppDbContext.Services.FirstOrDefault(t => t.Id == service.Id);
            if (existingService == null)
            {
                return NotFound();
            }
            existingService.Name = service.Name;
            mentorAppDbContext.SaveChanges();
            return RedirectToAction("Index", "Service");

        }

        public IActionResult Delete(int id)
        {
            var service = mentorAppDbContext.Services.FirstOrDefault(t => t.Id == id);
            if (service == null)
            {
                return NotFound();
            }


            mentorAppDbContext.Services.Remove(service);
            mentorAppDbContext.SaveChanges();

            return RedirectToAction("Index", "Service");
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var eService = mentorAppDbContext.Services.FirstOrDefault(t => t.Id == id);
            if (eService is null)
                return NotFound();

            return View(eService);
        }

    }
}
