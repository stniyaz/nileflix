using Microsoft.AspNetCore.Mvc;
using Movie.Business.Services.Interfaces;

namespace Movie.MVC.Controllers
{
    public class LiveController : Controller
    {
        private readonly ILiveService _liveService;

        public LiveController(ILiveService liveService)
        {
            _liveService = liveService;
        }
        public async Task<IActionResult> Index()
        {
            var lives = await _liveService.GetAllAsync(x => !x.IsDeleted);
            return View(lives);
        }
    }
}
