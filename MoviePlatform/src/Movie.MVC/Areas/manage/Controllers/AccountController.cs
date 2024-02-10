using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.Services.Interfaces;
using Movie.Business.ViewModels;

namespace Movie.MVC.Areas.manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                await _accountService.LoginAsync(model);
            }
            catch (LockedUserException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (UserInvalidCredentialsException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("index", "dashboard");
        }
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("login", "account");
        }
    }
}
