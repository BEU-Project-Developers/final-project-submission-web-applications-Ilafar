using Mentor.Models;
using Mentor.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Areas.Manage.Controllers;

[Area("Manage")]
public class LoginController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager) : Controller
{
    public async Task<IActionResult> CreateAdmin()
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
            var roleResult = await userManager.AddToRoleAsync(user, "Admin");
        }
        return Json(result);
    }

    public async Task<IActionResult> CreateRole()
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

    //[AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    //[AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(AdminLoginVm adminLoginVm)
    {
        if (!ModelState.IsValid)
            return View();
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
            ModelState.AddModelError("", "invalid username or password");
            return View();
        }

        await signInManager.SignInAsync(user, false);

        return RedirectToAction("Index","Dashboard");
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Login", "Login", "Manage");
    }
}