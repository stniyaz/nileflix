using Microsoft.AspNetCore.Mvc;
using Movie.Business.Services.Interfaces;

namespace Movie.MVC.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IMovieGenreService _movieGenreService;

        public MovieController(IMovieService movieService,
                               IMovieGenreService movieGenreService)
        {
            _movieService = movieService;
            _movieGenreService = movieGenreService;
        }
        public async Task<IActionResult> Detail(int id)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("pricing", "watch", new { id = id });
            var check = await _movieService.CheckVideoAndUser(id, User.Identity.Name);
            if (check) return RedirectToAction("watch", "movie", new { id = id });
            return RedirectToAction("pricing", "watch", new { id = id });
        }
        public async Task<IActionResult> Watch(int id)
        {
            ViewBag.MovieGenres = await _movieGenreService.GetAllIncludesAsync();
            var movie = await _movieService.GetMovieWithAllIncludes(id);
            if (movie is null) return NotFound();
            return View(movie);
        }
    }
}
