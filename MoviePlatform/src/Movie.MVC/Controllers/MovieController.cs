using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.CommentExceptions;
using Movie.Business.CustomExceptions.MoiveExceptions;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.DTOs.ComentDTOs;
using Movie.Business.Services.Interfaces;
using Movie.MVC.ViewModels;

namespace Movie.MVC.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IMovieGenreService _movieGenreService;
        private readonly ICommentService _commentService;
        private readonly ICommentReactionService _commentReaction;

        public MovieController(IMovieService movieService,
                               IMovieGenreService movieGenreService,
                               ICommentService commentService,
                               ICommentReactionService commentReaction)
        {
            _movieService = movieService;
            _movieGenreService = movieGenreService;
            _commentService = commentService;
            _commentReaction = commentReaction;
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
                return View(new WatchVM { Movie = movie });
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

        [HttpPost]
        public async Task<IActionResult> PostComment(CommentCreateDTO comment)
        {
            try
            {
                ViewBag.MovieGenres = await _movieGenreService.GetAllIncludesAsync();
                var movie = await _movieService.GetMovieWithAllIncludes(comment.MovieId);
                if (!ModelState.IsValid) return View("watch", new WatchVM { Movie = movie, Comment = comment });
                await _commentService.CreateAsync(comment);
                return RedirectToAction("watch", new { id = comment.MovieId });
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (MovieNotFoundException)
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
        [HttpPost]
        public async Task<IActionResult> LikeComment(int id)
        {
            try
            {
                var check = await _commentReaction.Like(id);
                if (check == true) return StatusCode(201);
                if (check == false) return StatusCode(204);
                return StatusCode(200);
            }
            catch (CommentNotFoundException)
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
        [HttpPost]
        public async Task<IActionResult> DislikeComment(int id)
        {
            try
            {
                var check = await _commentReaction.Dislike(id);
                if (check == true) return StatusCode(201);
                if (check == false) return StatusCode(204);
                return StatusCode(200);
            }
            catch (CommentNotFoundException)
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
