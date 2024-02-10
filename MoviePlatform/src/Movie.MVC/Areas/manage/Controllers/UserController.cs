using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Movie.MVC.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
