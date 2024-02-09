using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.CustomExceptions.GenreExceptions;
using Movie.Business.DTOs.GenreDTOs;
using Movie.Business.Helpers.Pagination;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;

namespace Movie.MVC.Areas.manage.Controllers
{
    [Area("manage")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly IMapper _mapper;
        private readonly IMovieGenreService _movieGenreService;

        public GenreController(IGenreService genreService,
                               IMapper mapper,
                               IMovieGenreService movieGenreService)
        {
            _genreService = genreService;
            _mapper = mapper;
            _movieGenreService = movieGenreService;
        }
        public async Task<IActionResult> Index(int? sortBy, string? search, string page = "1")
        {
            try
            {
                PaginatedList<Genre> genres = _genreService.SortBy(sortBy, search, page)
                     /* ?? await _genreService.GetAllAsync()*/;
                ViewBag.Movies = await _movieGenreService.GetAllAsync();

                return View(genres);
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
        public IActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Create(GenreCreateDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            try
            {
                await _genreService.CreateAsync(dto);
            }
            catch (ExistGenreException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (GenreImageContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (GenreImageLengthException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            var wanted = await _genreService.GetAsync(x => x.Id == id);
            if (wanted is null) return NotFound();
            var genreDto = _mapper.Map<GenreUpdateDTO>(wanted);
            return View(genreDto);
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Update(GenreUpdateDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            try
            {
                await _genreService.UpdateAsync(dto);
            }
            catch (GenreNotFoundException)
            {
                return NotFound();
            }
            catch (GenreImageLengthException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (GenreImageContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (ExistGenreException ex)
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
                await _genreService.HardDeleteAsync(id);
            }
            catch (GenreNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok();
        }
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                await _genreService.SoftDeleteAsync(id);
            }
            catch (GenreNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("index", "genre");
        }
    }
}
