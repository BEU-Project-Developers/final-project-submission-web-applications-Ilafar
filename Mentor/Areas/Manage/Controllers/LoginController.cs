using Mentor.Models;
using Mentor.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Areas.Manage.Controllers;

[Area("Manage")]
[Authorize(Roles = "Admin")]
public class LoginController(UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    RoleManager<IdentityRole> roleManager) : Controller
{
    public async Task<IActionResult> CreateAdmin()
    {
        try
        {
            AppUser user = new AppUser();
            user.UserName = "_azizli";
            user.FirstName = "admin";
            user.LastName = "loremov";
            user.Age = 21;
            user.Email = "admin@gmail.com";
            var result = await userManager.CreateAsync(user, "_Azizli2005");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
            return Json(result);
        }
        catch
        {
            return Problem();
        }
    }

    public async Task<IActionResult> CreateRole()
    {
        try
        {
            var roles = new[] { "Member", "Admin" };

            foreach (var roleName in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            return Content("Roles created successfully.");
        }
        catch
        {
            return Problem();
        }
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(AdminLoginVm adminLoginVm)
    {
        if (!ModelState.IsValid)
            return View();

        try
        {
            var user = await userManager.FindByNameAsync(adminLoginVm.Username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View();
            }

            var password = await userManager.CheckPasswordAsync(user, adminLoginVm.Password);
            if (!password)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View();
            }

            if (!await userManager.IsInRoleAsync(user, "Admin"))
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }

            await signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Dashboard");
        }
        catch
        {
            ModelState.AddModelError("", "Something went wrong");
            return View();
        }
    }

    public async Task<IActionResult> Logout()
    {
        try
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Login", "Manage");
        }
        catch
        {
            return Problem();
        }
    }
}
