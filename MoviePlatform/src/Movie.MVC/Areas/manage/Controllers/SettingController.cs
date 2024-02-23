using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.CustomExceptions.SettingExceptions;
using Movie.Business.DTOs.SettingDTOs;
using Movie.Business.Services.Interfaces;

namespace Movie.MVC.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly IMapper _mapper;

        public SettingController(ISettingService settingService,
                                 IMapper mapper)
        {
            _settingService = settingService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string? search)
        {
            try
            {
                if (search is not null) ViewBag.Search = search;
                return View(await _settingService.SearchByAsync(search));
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
        public async Task<IActionResult> Update(int id)
        {
            var wanted = await _settingService.GetAsync(x => x.Id == id);
            if (wanted is null) return NotFound();
            var dto = _mapper.Map<SettingUpdateDTO>(wanted);
            return View(dto);
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Update(SettingUpdateDTO dto)
        {
            if(!ModelState.IsValid) return View(dto);
            try
            {
                await _settingService.Update(dto);
                return RedirectToAction("index");
            }
            catch (SettingNotFoundException)
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
