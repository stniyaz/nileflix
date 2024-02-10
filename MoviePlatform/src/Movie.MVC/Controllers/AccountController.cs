using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.Services.Interfaces;
using Movie.Business.ViewModels;

namespace Movie.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> SignUp(RegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                var result = await _accountService.RegisterAsync(model);
                if(result.Succeeded)
                {
                    //var code = await 
                }
            }
            catch (UnacceptablePrivacyException ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                return View(model);
            }
            catch (ExistUsernameException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(model);
            }
            catch (ExistEmailException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(model);
            }
            catch (UnexceptedException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("index", "home");
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> SignIn(LoginVM model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                await _accountService.LoginAsync(model);
            }
            catch (BannedUserException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
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
            return RedirectToAction("index", "home");
        }
    }
}
