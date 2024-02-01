using Microsoft.AspNetCore.Mvc;

namespace Movie.MVC.Controllers
{
	public class ErrorController : Controller
	{
		public IActionResult Error()
		{
			return View();
		}
	}
}
