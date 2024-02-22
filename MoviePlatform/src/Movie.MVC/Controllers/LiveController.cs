using Microsoft.AspNetCore.Mvc;

namespace Movie.MVC.Controllers
{
    public class LiveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
