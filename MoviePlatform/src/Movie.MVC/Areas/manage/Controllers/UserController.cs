using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.Services.Interfaces;

namespace Movie.MVC.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IAccountService _accountService;

        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<IActionResult> Index(string? search)
        {
            try
            {
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
        public async Task<IActionResult> Moderator(string? search)
        {
            try
            {
                var users = await _accountService.SearchByModsAsync(search);
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
            if(user == null) return NotFound();
            return View(user);
        }
    }
}
