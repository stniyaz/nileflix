using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movie.Core.Models;

namespace Movie.MVC.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        //private readonly UserManager<AppUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        //public DashboardController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    _userManager = userManager;
        //    _roleManager = roleManager;
        //}
        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> CreateRole()
        //{
        //    IdentityRole role1 = new IdentityRole("User");

        //    await _roleManager.CreateAsync(role1);

        //    return Ok();
        //}
        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser admin = new AppUser()
        //    {
        //        FirstName = "Niyaz",
        //        LastName = "Soltanov",
        //        UserName = "nilexadmin",
        //        CreatedDate = DateTime.UtcNow.AddHours(4),
        //        Email = "n.soltanov13@gmail.com"
        //    };

        //    await _userManager.CreateAsync(admin, "Salam123!");

        //    await _userManager.AddToRoleAsync(admin, "Admin");

        //    return Ok();
        //}
    }
}
