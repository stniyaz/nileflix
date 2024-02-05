using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.MoiveExceptions;
using Movie.Business.DTOs.MovieDTOs;
using Movie.Business.Services.Interfaces;

namespace Movie.MVC.Areas.manage.Controllers
{
	[Area("manage")]
	public class MovieController : Controller
	{
		private readonly IMovieService _movieService;
		private readonly ICountryService _countryService;
		private readonly IGenreService _genreService;

		public MovieController(IMovieService movieService,
							   ICountryService countryService,
							   IGenreService genreService)
		{
			_movieService = movieService;
			_countryService = countryService;
			_genreService = genreService;
		}
		public async Task<IActionResult> Index()
		{
			var movies = await _movieService.GetAllAsync(null, "MovieGenres", "MovieImages");
			return View(movies);
		}
		public async Task<IActionResult> Create()
		{
			ViewBag.Countires = await _countryService.GetAllAsync();
			ViewBag.Genres = await _genreService.GetAllAsync();

			return View();
		}
		[ValidateAntiForgeryToken, HttpPost]
		public async Task<IActionResult> Create(MovieCreateDTO dto)
		{
			ViewBag.Countires = await _countryService.GetAllAsync();
			ViewBag.Genres = await _genreService.GetAllAsync();

			if (!ModelState.IsValid) return View(dto);

			try
			{
				await _movieService.Create(dto);
			}
			catch(InvalidCountryIdException ex)
			{
				ModelState.AddModelError(ex.PropertyName, ex.Message);
				return View(dto);
			}
            catch (InvalidGenreIdException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (MovieVideoContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (MovieImageContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (MovieImageLengthException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (Exception)
			{
				throw;
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
