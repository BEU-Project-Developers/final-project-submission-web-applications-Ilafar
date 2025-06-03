using Mentor.DAL;
using Mentor.Models;
using Mentor.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Security.Claims;

namespace Mentor.Controllers
{
    public class PricingController(MentorAppDbContext mentorAppDbContext, UserManager<AppUser> userManager) : Controller
    {
        public IActionResult Index()
        {
            try
            {
                var pricing = mentorAppDbContext.Pricings
                    .Include(p => p.PricingServices)
                    .ThenInclude(ps => ps.Service)
                    .ToList();

                var service = mentorAppDbContext.Services.ToList();

                int? selectedPricingId = null;

                if (User.Identity.IsAuthenticated)
                {
                    var userId = userManager.GetUserId(User);
                    selectedPricingId = mentorAppDbContext.UserPricing
                        .FirstOrDefault(up => up.UserId == userId)?.PricingId;
                }

                PricingVm pricingVm = new PricingVm()
                {
                    Pricings = pricing,
                    Services = service,
                    SelectedPricingId = selectedPricingId
                };

                ViewData["ActivePage"] = "Pricing";
                return View(pricingVm);
            }
            catch
            {
                return Problem();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectPricing(int pricingId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return RedirectToAction("Login", "GetStarted",
                        new { returnUrl = Url.Action("Index", "Pricing") });
                }

                var userPricing = await mentorAppDbContext.UserPricing
                    .FirstOrDefaultAsync(up => up.UserId == userId);

                if (userPricing != null)
                {
                    userPricing.PricingId = pricingId;
                    userPricing.IsFeatured = true;
                    Debug.WriteLine("Existing UserPricing found. Updated PricingId to: " + pricingId);
                }
                else
                {
                    var newUserPricing = new UserPricing
                    {
                        UserId = userId,
                        PricingId = pricingId,
                        IsFeatured = true
                    };

                    await mentorAppDbContext.UserPricing.AddAsync(newUserPricing);
                    Debug.WriteLine("New UserPricing created with PricingId = " + pricingId);
                }

                await mentorAppDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                return Problem();
            }
        }
    }
}
