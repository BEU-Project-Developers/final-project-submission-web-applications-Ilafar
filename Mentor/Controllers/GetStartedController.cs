using Mentor.Models;
using Mentor.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mentor.Controllers
{
    public class GetStartedController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager) : Controller
    {
        public IActionResult Started()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Started(StartVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await userManager.FindByNameAsync(model.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(model.Username), "This username is already taken");
                return View(model);
            }

            var newUser = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Username,
                Age = model.Age,
            };

            var creationResult = await userManager.CreateAsync(newUser, model.Password);

            if (!creationResult.Succeeded)
            {
                foreach (var error in creationResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            // await userManager.AddToRoleAsync(newUser, "Member");

            return RedirectToAction("Login", "Getstarted");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (!ModelState.IsValid)
                return View();
            var eUser = await userManager.FindByNameAsync(loginVm.Username);
            if (eUser is null)
            {
                ModelState.AddModelError("", "invalid username or passworddd");
                return View();
            }
            var resultPassword = await signInManager.PasswordSignInAsync(
    eUser.UserName,  
    loginVm.Password,
    isPersistent: false,   
    lockoutOnFailure: true
);

            if (resultPassword.IsLockedOut)
            {
                ModelState.AddModelError("", "try some time later");
                return View();
            }

            if (!resultPassword.Succeeded)
            {
                ModelState.AddModelError("", "invalid username or password");
                return View();
            }

            return RedirectToAction("index","home");
        }
    }
}