using Mentor.DAL;
using Mentor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MentorAppDbContext>(opt =>
{
    opt.UseSqlServer("Server=DESKTOP-Q4CUAVA\\SQLEXPRESS;Database=MentorIlafer;Trusted_Connection=True;TrustServerCertificate=True;");
});
builder.Services.AddIdentity<AppUser, IdentityRole>(option =>
{
    option.Password.RequireLowercase = true;
    option.Password.RequireUppercase = true;
    option.Password.RequireDigit = true;
    option.Password.RequiredLength = 10;
    option.SignIn.RequireConfirmedEmail = false;
    option.Lockout.MaxFailedAccessAttempts = 3;
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
}).AddEntityFrameworkStores<MentorAppDbContext>();

builder.Services.ConfigureApplicationCookie(option =>
{
    option.Events.OnRedirectToLogin = option.Events.OnRedirectToAccessDenied = context =>
    {
        var uri = new Uri(context.RedirectUri);
        if (context.Request.Path.Value.ToLower().StartsWith("/manage"))
        {
            context.Response.Redirect("/manage/login/login" + uri.Query);
        }
        else
        {
            context.Response.Redirect("/getstarted/login" + uri.Query);

        }
        return Task.CompletedTask;
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
