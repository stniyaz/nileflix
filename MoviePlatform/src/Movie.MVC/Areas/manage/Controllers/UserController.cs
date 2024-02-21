using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.DTOs.UserDTOs;
using Movie.Business.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace Movie.MVC.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public UserController(IAccountService accountService,
                              IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string? search)
        {
            try
            {
                ViewBag.Search = search;
                var users = await _accountService.SearchByUsersAsync(search);
                return View(users);
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
        public async Task<IActionResult> Banned(string? search)
        {
            try
            {
                var users = await _accountService.SearchByBannedUsersAsync(search);
                return View(users);
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
        public async Task<IActionResult> HardDelete(string name)
        {
            try
            {
                await _accountService.DeleteByNameAsync(name);
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok();
        }
        public async Task<IActionResult> Update(string name)
        {
            ViewBag.Roles = await _accountService.GetRolesAsync();
            var user = await _accountService.GetUserByNameAsync(name);
            if (user == null) return NotFound();
            var model = _mapper.Map<UserUpdateDTO>(user);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDTO dto)
        {
            ViewBag.Roles = await _accountService.GetRolesAsync();
            if (!ModelState.IsValid) return View(dto);
            try
            {
                await _accountService.UpdateAsync(dto);
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (ExistEmailException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (ExistUsernameException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (InvalidRoleIdException ex)
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
