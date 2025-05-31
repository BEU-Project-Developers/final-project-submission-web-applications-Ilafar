using Mentor.Models;
using Mentor.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

             await userManager.AddToRoleAsync(newUser, "Member");

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
                ModelState.AddModelError("", "invalid username or password");
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
                ModelState.AddModelError(string.Empty, "try some time later");
                return View();
            }

            if (!resultPassword.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "invalid username or password");
                return View();
            }

            return RedirectToAction("index","home");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile()
        {
            var eUser = await userManager.GetUserAsync(User);
            if (eUser == null) return NotFound();

            var profileVm = new ProfileVm
            {
                Username = eUser.UserName,
                FirstName = eUser.FirstName,
                LastName = eUser.LastName,
                Age = eUser.Age
            };

            return View(profileVm);
        }
        [Authorize(Roles = "Member")]

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileVm profileVm)
        {
            if (!ModelState.IsValid)
                return View(profileVm);

            var updateUser = await userManager.GetUserAsync(User);
            if (updateUser == null) return NotFound();

            updateUser.FirstName = profileVm.FirstName;
            updateUser.LastName = profileVm.LastName;
            updateUser.UserName = profileVm.Username;
            updateUser.Age = profileVm.Age;

            if (!string.IsNullOrWhiteSpace(profileVm.NewPassword))
            {
                if (string.IsNullOrWhiteSpace(profileVm.Password))
                {
                    ModelState.AddModelError("Password", "Write previous password");
                    return View(profileVm);
                }

                var passwordChangeResult = await userManager.ChangePasswordAsync(updateUser, profileVm.Password, profileVm.NewPassword);
                if (!passwordChangeResult.Succeeded)
                {
                    foreach (var error in passwordChangeResult.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(profileVm);
                }
            }

            var updateResult = await userManager.UpdateAsync(updateUser);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(profileVm);
            }

            await signInManager.SignInAsync(updateUser, true);
            return RedirectToAction(nameof(Profile));
        }

    }
}