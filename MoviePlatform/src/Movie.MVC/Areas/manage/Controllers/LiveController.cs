using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.LiveExceptions;
using Movie.Business.DTOs.LiveDTOs;
using Movie.Business.Services.Interfaces;

namespace Movie.MVC.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class LiveController : Controller
    {
        private readonly ILiveService _liveService;
        private readonly IMapper _mapper;

        public LiveController(ILiveService liveService,
                              IMapper mapper)
        {
            _liveService = liveService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string search)
        {
            ViewBag.Search = search;
            var lives = await _liveService.GetAllAsync();
            return View(lives);
        }
        public IActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Create(LiveCreateDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            try
            {
                await _liveService.CreateAsync(dto);
            }
            catch (LiveContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (LiveImageLengthException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var wanted = await _liveService.GetAsync(x => x.Id == id);
            if (wanted is null) return NotFound();
            var liveDto = _mapper.Map<LiveUpdateDTO>(wanted);
            return View(liveDto);
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Update(LiveUpdateDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            try
            {
                await _liveService.UpdateAsync(dto);
            }
            catch (LiveContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (LiveImageLengthException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _liveService.DeleteAsync(id);
            }
            catch (LiveNotFoundException)
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
                await _liveService.SoftDelete(id);
            }
            catch (LiveNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("index");
        }

    }
}
