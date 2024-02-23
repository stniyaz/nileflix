using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Movie.Business.CustomExceptions.MoiveExceptions;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;
using Movie.Core.Repositories;
using System.Linq.Expressions;

namespace Movie.Business.Services.Implementations
{
    public class UserWatchedMovieService : IUserWatcedMovieService
    {
        private readonly IUserWatcedMovieRepository _userWatcedMovieRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMovieService _movieService;
        private readonly IAccountService _accountService;

        public UserWatchedMovieService(IUserWatcedMovieRepository userWatcedMovieRepository,
                                       IHttpContextAccessor httpContext,
                                       IMovieService movieService,
                                       IAccountService accountService)
        {
            _userWatcedMovieRepository = userWatcedMovieRepository;
            _httpContext = httpContext;
            _movieService = movieService;
            _accountService = accountService;
        }

        public async Task<List<UserWatchedMovie>> GetAllAsync(Expression<Func<UserWatchedMovie, bool>>? expression = null, params string[]? includes)
        {
            return await _userWatcedMovieRepository.GetAllAsync(expression, includes).ToListAsync();
        }

        public async Task ViewCounterAsync(int movieId)
        {
            var movie = await _movieService.GetAsync(x => x.Id == movieId);
            if (movie == null) throw new MovieNotFoundException();
            var user = await _accountService.GetUserByNameAsync(_httpContext.HttpContext.User.Identity.Name);
            if (user is null) throw new UserNotFoundException();

            if (!_userWatcedMovieRepository.Table.Any(x => x.AppUserId == user.Id && x.MovieId == movie.Id))
            {
                var watchedMovie = new UserWatchedMovie { MovieId = movie.Id, AppUserId = user.Id };
                await _userWatcedMovieRepository.CreateAsync(watchedMovie);
                movie.Views++;
            }
            await _userWatcedMovieRepository.CommitAsync();
        }
    }
}
