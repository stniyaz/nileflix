using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.GenreExceptions;
using Movie.Business.DTOs.GenreDTOs;
using Movie.Business.Services.Interfaces;

namespace Movie.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenreService _service;

        public HomeController(IGenreService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenreCreateDTO dto)
        {
            if(!ModelState.IsValid) return View(dto);
            try
            {
                await _service.CreateAsync(dto);
            }
            catch(GenreImageContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (GenreImageLengthException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
    }
}
