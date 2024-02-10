using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.CustomExceptions.MoiveExceptions;
using Movie.Business.DTOs.MovieDTOs;
using Movie.Business.Services.Interfaces;
using Movie.MVC.Areas.manage.ViewModels;

namespace Movie.MVC.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ICountryService _countryService;
        private readonly IGenreService _genreService;
        private readonly IMovieGenreService _movieGenreService;
        private readonly IMapper _mapper;
        private readonly IMovieImageService _movieImageService;

        public MovieController(IMovieService movieService,
                               ICountryService countryService,
                               IGenreService genreService,
                               IMovieGenreService movieGenreService,
                               IMapper mapper,
                               IMovieImageService movieImageService)
        {
            _movieService = movieService;
            _countryService = countryService;
            _genreService = genreService;
            _movieGenreService = movieGenreService;
            _mapper = mapper;
            _movieImageService = movieImageService;
        }
        public async Task<IActionResult> Index(int? sortBy, string? search, string page = "1")
        {
            try
            {
                string? sortByValue = string.Empty;
                string? refererUrl = HttpContext.Request.Headers["Referer"];
                if (refererUrl != null)
                {
                    Uri? uri = new Uri(refererUrl);
                    sortByValue = System.Web.HttpUtility.ParseQueryString(uri.Query)?.Get("sortBy");
                    search = System.Web.HttpUtility.ParseQueryString(uri.Query)?.Get("search")
                          ?? search;
                    //page = System.Web.HttpUtility.ParseQueryString(uri.Query)?.Get("page")
                    //    ?? page;
                }
                if (int.TryParse(sortByValue, out int intSort))
                {
                    sortBy = intSort;
                }

                MovieIndexVM model = new MovieIndexVM
                {
                    MovieGenres = await _movieGenreService.GetAllAsync(null, "Genre"),
                    PaginatedMovies = _movieService.SortByAsync(sortBy, search, page)
                };
                return View(model);
            }
            catch (InvalidSearchException)
            {
                return NotFound();
            }
            catch (InvalidSortByIdException)
            {
                return NotFound();
            }
            catch (InvalidPageException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
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
                await _movieService.CreateAsync(dto);
            }
            catch (InvalidCountryIdException ex)
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
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                await _movieService.SoftDeleteAsync(id);
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            var wanted = await _movieService.GetAsync(x => x.Id == id);
            if (wanted is null)
                return NotFound();

            ViewBag.Countires = await _countryService.GetAllAsync();
            ViewBag.Genres = await _genreService.GetAllAsync();
            ViewBag.Images = await _movieImageService.GetAllAsync(x => x.MovieId == id);
            ViewBag.MovieGenres = await _movieGenreService.GetAllAsync();
            ViewBag.Movie = await _movieService.GetAsync(x => x.Id == id);

            var dto = _mapper.Map<MovieUpdateDTO>(wanted);
            return View(dto);
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Update(MovieUpdateDTO dto)
        {
            ViewBag.Countires = await _countryService.GetAllAsync();
            ViewBag.Genres = await _genreService.GetAllAsync();
            ViewBag.Images = await _movieImageService.GetAllAsync(x => x.MovieId == dto.Id);
            ViewBag.Movie = await _movieService.GetAsync(x => x.Id == dto.Id);
            ViewBag.MovieGenres = await _movieGenreService.GetAllAsync();
            if (!ModelState.IsValid) return View(dto);

            try
            {
                await _movieService.UpdateAsync(dto);
            }
            catch (InvalidCountryIdException ex)
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
        public async Task<IActionResult> HardDelete(int id)
        {
            try
            {
                await _movieService.HardDeleteAsync(id);
            }
            catch (MovieNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
