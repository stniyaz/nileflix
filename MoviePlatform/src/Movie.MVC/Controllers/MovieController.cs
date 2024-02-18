using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.MoiveExceptions;
using Movie.Business.CustomExceptions.UserException;
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
            try
            {
                if (!User.Identity.IsAuthenticated) return RedirectToAction("pricing", "movie", new { id = id });
                var check = await _movieService.CheckVideoAndUser(id, User.Identity.Name);
                if (check) return RedirectToAction("watch", "movie", new { id = id });
                return RedirectToAction("pricing", "movie", new { id = id });
            }
            catch (MovieNotFoundException)
            {
                return NotFound();
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IActionResult> Watch(int id)
        {
            try
            {
                if (!User.IsInRole("User") && !User.IsInRole("Admin") && !User.IsInRole("Moderator"))
                {
                    return RedirectToAction("signin", "account");
                }
                ViewBag.MovieGenres = await _movieGenreService.GetAllIncludesAsync();
                var check = await _movieService.CheckVideoAndUser(id, User.Identity.Name);
                if (!check) return RedirectToAction("pricing", "movie", new { id = id });
                var movie = await _movieService.GetMovieWithAllIncludes(id);
                if (movie is null) return NotFound();
                return View(movie);
            }
            catch (MovieNotFoundException)
            {
                return NotFound();
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> Pricing(int id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var check = await _movieService.CheckVideoAndUser(id, User?.Identity?.Name);
                    if (check) return RedirectToAction("watch", "movie", new { id = id });
                }
                ViewBag.MovieGenres = await _movieGenreService.GetAllIncludesAsync();
                var movie = await _movieService.GetMovieWithAllIncludes(id);
                return View(movie);
            }
            catch (MovieNotFoundException)
            {
                return NotFound();
            }
            catch (UserNotFoundException)
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
