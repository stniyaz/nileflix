using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;
using Movie.MVC.Areas.manage.ViewModels;

namespace Movie.MVC.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMovieService _movieService;
        private readonly ICommentService _commentService;
        private readonly IUserWatcedMovieService _userWatcedMovieService;
        private readonly IEarningService _earningService;

        public DashboardController(UserManager<AppUser> userManager,
                                   IMovieService movieService,
                                   ICommentService commentService,
                                   IUserWatcedMovieService userWatcedMovieService,
                                   IEarningService earningService)
        {
            _userManager = userManager;
            _movieService = movieService;
            _commentService = commentService;
            _userWatcedMovieService = userWatcedMovieService;
            _earningService = earningService;
        }
        public async Task<IActionResult> Index()
        {
            var date = DateTime.UtcNow.AddHours(4).AddDays(-30);
            var comments = await _commentService.GetCommentsAsync(x => x.CreatedDate > date);
            var views = await _userWatcedMovieService.GetAllAsync(x => x.CreatedDate > date);
            DashboardVM model = new DashboardVM()
            {
                Movies = await _movieService.GetAllAsync(x => x.CreatedDate > date),
                Users = await _userManager.Users.Where(x => x.CreatedDate > date).ToListAsync(),
                CommentsCount = comments.Count,
                ViewsCount = views.Count,
                Earned = await _earningService.GetAllEarnedMoneyAsync(x => x.CreatedDate > date),
            };
            return View(model);
        }
    }
}
