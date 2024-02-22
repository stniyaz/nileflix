using Microsoft.AspNetCore.Mvc;
using Movie.Business.Services.Interfaces;

namespace Movie.MVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IGenreService _genreService;

        public CatalogController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        public async Task<IActionResult> Index()
        {
            var genres = await _genreService.GetAllAsync(null, "MovieGenres");
            return View(genres);
        }
    }
}
