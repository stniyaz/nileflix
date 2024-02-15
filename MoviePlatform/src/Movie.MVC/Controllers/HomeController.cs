using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.CustomExceptions.MoiveExceptions;
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
        public async Task<IActionResult> Index(string? search, int? genreId = 0)
        {
            try
            {
                ViewBag.Search = search;
                HomeVM model = new HomeVM
                {
                    Movies = await _movieService.GetAllHome(genreId, search),
                    Genres = await _genreService.GetAllAsync(),
                    MovieGenres = await _movieGenreService.GetAllIncludesAsync(),
                    GenreId = genreId,
                };
                return View(model);
            }
            catch (InvalidGenreIdException)
            {
                return NotFound();
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
