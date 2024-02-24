using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.CustomExceptions.MoiveExceptions;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.DTOs.UserDTOs;
using Movie.Business.Helpers.Mail;
using Movie.Business.Services.Interfaces;
using Movie.Business.ViewModels;
using Movie.MVC.ViewModels;

namespace Movie.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;
        private readonly IMovieGenreService _movieGenreService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IUserSavedMovieService _userSavedMovieService;
        private readonly IPaymentService _paymentService;
        private readonly ILiveService _liveService;

        public HomeController(IMovieService movieService,
                              IGenreService genreService,
                              IMovieGenreService movieGenreService,
                              IAccountService accountService,
                              IMapper mapper,
                              IEmailService emailService,
                              IUserSavedMovieService userSavedMovieService,
                              IPaymentService paymentService,
                              ILiveService liveService)
        {
            _movieService = movieService;
            _genreService = genreService;
            _movieGenreService = movieGenreService;
            _accountService = accountService;
            _mapper = mapper;
            _emailService = emailService;
            _userSavedMovieService = userSavedMovieService;
            _paymentService = paymentService;
            _liveService = liveService;
        }
        private readonly string _stripeSecret = "whsec_34a2ca4cb8579d9bb7a4efdd0c33cd559927cce1eb83f73b66582ea3c2c3977a";
        public async Task<IActionResult> Index(string? search, int? genreId = 0)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                    ViewBag.SavedIds = await _userSavedMovieService.GetUserSavedMoviesIdsAsync(User?.Identity?.Name);
                ViewBag.Search = search;
                HomeVM model = new HomeVM
                {
                    Movies = await _movieService.GetAllHome(genreId, search),
                    Genres = await _genreService.GetAllAsync(),
                    MovieGenres = await _movieGenreService.GetAllIncludesAsync(),
                    GenreId = genreId,
                    Lives = await _liveService.GetAllAsync()
                };
                return View(model);
            }
            catch (InvalidGenreIdException)
            {
                return NotFound();
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
        public async Task<IActionResult> Profile()
        {
            if (!User.IsInRole("User") && !User.IsInRole("Admin") && !User.IsInRole("Moderator"))
            {
                return RedirectToAction("signin", "account");
            }
            var activeUser = await _accountService.GetUserByNameAsync(User.Identity.Name);
            return View(activeUser);
        }
        public async Task<IActionResult> Favorite()
        {
            if (!User.IsInRole("User") && !User.IsInRole("Admin") && !User.IsInRole("Moderator"))
            {
                return RedirectToAction("signin", "account");
            }
            FavoriteVM model = new FavoriteVM
            {
                ActiveUser = await _accountService.GetUserByNameAsync(User.Identity.Name),
                Movies = await _userSavedMovieService.GetUserSavedMoviesAsync(User.Identity.Name),
                MovieGenres = await _movieGenreService.GetAllIncludesAsync(),
            };
            return View(model);
        }
        public async Task<IActionResult> Detail()
        {
            if (!User.IsInRole("User") && !User.IsInRole("Admin") && !User.IsInRole("Moderator"))
            {
                return RedirectToAction("signin", "account");
            }
            var user = await _accountService.GetUserByNameAsync(User.Identity.Name);
            var dto = _mapper.Map<UserEditDTO>(user);
            return View(dto);
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Detail(UserEditDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            try
            {
                var vm = await _accountService.UserEditAsync(dto);
                if (vm.IsChanged)
                {
                    var confiramtionLink = Url.Action(nameof(ChangeEmail), "Home",
                                                  new { vm.UserId, vm.NewMail, vm.Token }, Request.Scheme);

                    var message = new Message(new string[]
                                             { vm.NewMail }, "nileX confiramtion", $"Please click on the link to confirm your account: {confiramtionLink}");
                    await _emailService.SendMailAsync(message);
                    return StatusCode(204);
                }
                else
                {
                    return new StatusCodeResult(202);
                }
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
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> ChangeEmail(string userId, string newMail, string token)
        {
            try
            {
                await _emailService.ChangeEmailAsync(userId, newMail, token);
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
            return RedirectToAction(nameof(Detail));
        }
        public async Task<IActionResult> ChangePassword()
        {
            if (!User.IsInRole("User") && !User.IsInRole("Admin") && !User.IsInRole("Moderator"))
            {
                return RedirectToAction("signin", "account");
            }
            ViewBag.User = await _accountService.GetUserByNameAsync(User.Identity.Name);
            return View();
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            ViewBag.User = await _accountService.GetUserByNameAsync(User.Identity.Name);

            if (!ModelState.IsValid) return View(model);
            try
            {
                await _accountService.ChangePasswordAsync(User.Identity.Name, model);
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (UserInvalidCredentialsException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
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
            return RedirectToAction(nameof(Profile));
        }
        public async Task<IActionResult> AddOrRemoveFavorite(int id)
        {
            if (!User.IsInRole("User") && !User.IsInRole("Admin") && !User.IsInRole("Moderator"))
            {
                return RedirectToAction("signin", "account");
            }
            try
            {
                var check = await _userSavedMovieService.AddOrRemoveAsync(id);
                if (check) return StatusCode(200);
                else return StatusCode(204);
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (MovieNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                throw;
            }
        }
        public IActionResult IsAuthenticated()
        {
            bool check = User.Identity.IsAuthenticated;
            return Json(new { loggedIn = check });
        }
        public IActionResult PricingPlans()
        {
            return View();
        }
        public async Task<IActionResult> Checkout(string date)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var model = await _paymentService.PaymentProcess(User.Identity.Name, date);
                    Response.Headers.Add("Location", model.Session.Url);
                    return new StatusCodeResult(303);
                }
                return RedirectToAction("signin", "account");
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (UnexceptedException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public async Task<IActionResult> c91d8291b1cd4893b870173f92636708(string userId, int amount)
        {
            try
            {
                await _accountService.UserToPremiumAsync(userId, amount);
                return RedirectToAction(nameof(Index));
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (UnexceptedException)
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
