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

        public DashboardController(UserManager<AppUser> userManager,
                                   IMovieService movieService,
                                   ICommentService commentService)
        {
            _userManager = userManager;
            _movieService = movieService;
            _commentService = commentService;
        }
        public async Task<IActionResult> Index()
        {
            var date = DateTime.UtcNow.AddHours(4).AddDays(-7);
            var comments = await _commentService.GetCommentsAsync();
            DashboardVM model = new DashboardVM()
            {
                Movies = await _movieService.GetAllAsync(x => x.CreatedDate > date),
                Users = await _userManager.Users.Where(x => x.CreatedDate > date).ToListAsync(),
                CommentsCount = comments.Count,
            };
            return View(model);
        }
    }
}
