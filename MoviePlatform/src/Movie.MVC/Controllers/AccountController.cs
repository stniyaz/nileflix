﻿using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.DTOs.UserDTOs;
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
            catch (NullDatasException)
            {
                return NotFound();
            }
            catch (UnsuccessfulConfirmationException ex)
            {
                return Ok(ex.Message);
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> ForgotPassword(UserResetPasswordDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            try
            {
                var vm = await _accountService.CheckEmailAsync(dto);
                var confiramtionLink = Url.Action(nameof(ResetPassword), "Account",
                                                  new { vm.Token, vm.Email }, Request.Scheme);

                var message = new Message(new string[]
                                         { vm.Email }, "nileX Reset", $"Please click on the link to reset your password: {confiramtionLink}");
                await _emailService.SendMailAsync(message);
            }
            catch (UserNotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (BannedUserException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(dto);
            }
            catch (Exception)
            {
                throw;
            }
            dto.IsSent = true;
            return View(dto);
        }
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            try
            {
                await _accountService.GetUserByEmailAsync(email);
                ResetPasswordVM vm = new ResetPasswordVM()
                {
                    Email = email,
                    Token = token
                };
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid) return View();
            try
            {
                await _accountService.ResetPasswordAsync(model);
            }
            catch (NullDatasException)
            {
                return NotFound();
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (UnexceptedException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction(nameof(SignIn));
        }
    }
}
