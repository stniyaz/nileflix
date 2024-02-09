using Microsoft.AspNetCore.Mvc;
using Movie.Business.Services.Interfaces;
using Movie.MVC.ViewModels;

namespace Movie.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;
        private readonly IMovieGenreService _movieGenreService;

        public HomeController(IMovieService movieService,
                              IGenreService genreService,
                              IMovieGenreService movieGenreService)
        {
            _movieService = movieService;
            _genreService = genreService;
            _movieGenreService = movieGenreService;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM model = new HomeVM
            {
                Movies = await _movieService.GetAllIncludesAsync(),
                Genres = await _genreService.GetAllAsync(),
                MovieGenres = await _movieGenreService.GetAllIncludesAsync(),
            };
            return View(model);
        }
    }
}
