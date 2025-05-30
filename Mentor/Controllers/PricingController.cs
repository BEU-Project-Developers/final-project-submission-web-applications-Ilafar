using Mentor.DAL;
using Mentor.Models;
using Mentor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Mentor.Controllers
{
    public class PricingController(MentorAppDbContext mentorAppDbContext) : Controller
    {
        public IActionResult Index()
        {
            var pricing = mentorAppDbContext.Pricings.Include(p => p.PricingServices).ThenInclude(ps => ps.Service).ToList();
            var service = mentorAppDbContext.Services.ToList();
            PricingVm pricingVm = new PricingVm()
            {
                Pricings = pricing,
                Services = service
            };
            ViewData["ActivePage"] = "Pricing";
            return View(pricingVm);
        }
    }
}
