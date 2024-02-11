using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.Helpers.Mail;
using Movie.Business.Services.Interfaces;
using Movie.Business.ViewModels;

namespace Movie.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;

        public AccountController(IAccountService accountService,
                                 IEmailService emailService)
        {
            _accountService = accountService;
            _emailService = emailService;
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
                var vm = await _accountService.RegisterAsync(model);
                var confiramtionLink = Url.Action(nameof(ConfirmEmail), "Account",
                                                  new { vm.Token, vm.Email }, Request.Scheme);

                var message = new Message(new string[]
                                         { vm.Email }, "nileX confiramtion", $"Please click on the link to confirm your account: {confiramtionLink}");
                await _emailService.SendMailAsync(message);
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
            return Ok("Please check your e-mail address. An activation message has been sent to your e-mail address.");
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            try
            {
                await _emailService.CheckConfirmationAsync(token, email);
            }
            catch (UnsuccessfulConfirmationException)
            {
                Ok("Something went wrong.");
            }
            catch (UnexceptedException)
            {
                Ok("Something went wrong.");
            }
            catch (Exception)
            {
                throw;
            }
            return Ok("Your account has been successfully activated.");
        }
    }
}
