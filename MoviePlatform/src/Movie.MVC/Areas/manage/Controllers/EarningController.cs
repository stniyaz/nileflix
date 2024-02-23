using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.Services.Interfaces;

namespace Movie.MVC.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class EarningController : Controller
    {
        private readonly IEarningService _earningService;

        public EarningController(IEarningService earningService)
        {
            _earningService = earningService;
        }
        public async Task<IActionResult> Index(string? search)
        {
            try
            {
                if (search is not null) ViewBag.Search = search;
                return View(await _earningService.GetSearchByAsync(search));
            }
            catch (InvalidSearchException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
