using System.Diagnostics;
using Mentor.DAL;
using Mentor.Models;
using Mentor.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mentor.Controllers
{
    public class HomeController(MentorAppDbContext mentorAppDbContext) : Controller
    {
        public async Task<int> MemberCount()
        {
            var count = await (from user in mentorAppDbContext.Users
                               join userRole in mentorAppDbContext.UserRoles on user.Id equals userRole.UserId
                               join role in mentorAppDbContext.Roles on userRole.RoleId equals role.Id
                               where role.Name == "Member"
                               select user).CountAsync();

            return count;
        }
        public async Task<IActionResult> Index()
        {


            HomeVm vm = new HomeVm()
            {
                Courses = mentorAppDbContext.Courses.Include(c => c.Trainer).ToList(),
                Trainers = mentorAppDbContext.Trainers.ToList(),
                Users = mentorAppDbContext.AppUsers.ToList(),
                MentorTags = mentorAppDbContext.MentorTags.ToList(),
                Count = await MemberCount(),
                WhyUses = mentorAppDbContext.WhyUs.ToList()
                
            };
            ViewData["ActivePage"] = "Home";
            return View(vm);
        }
       
        
    }
}
