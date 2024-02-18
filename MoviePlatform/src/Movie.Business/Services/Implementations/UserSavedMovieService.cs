using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Movie.Business.CustomExceptions.MoiveExceptions;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;
using Movie.Core.Repositories;

namespace Movie.Business.Services.Implementations
{
    public class UserSavedMovieService : IUserSavedMovieService
    {
        private readonly IUserSavedMovieRepository _userSavedMovieRepository;
        private readonly IMovieService _movieService;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContext;

        public UserSavedMovieService(IUserSavedMovieRepository userSavedMovieRepository,
                                     IMovieService movieService,
                                     IAccountService accountService,
                                     IHttpContextAccessor httpContext)
        {
            _userSavedMovieRepository = userSavedMovieRepository;
            _movieService = movieService;
            _accountService = accountService;
            _httpContext = httpContext;
        }
        public async Task<bool> AddOrRemoveAsync(int movieId)
        {
            bool check = true;
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // check
                var user = await _accountService.GetUserByNameAsync(_httpContext?.HttpContext?.User?.Identity?.Name);
                if (user is null) throw new UserNotFoundException();
                var movie = await _movieService.GetAsync(x => x.Id == movieId);
                if (movie is null) throw new MovieNotFoundException();

                // add or remove
                if (_userSavedMovieRepository.Table.Any(x => x.MovieId == movieId &&
                                                             x.AppUserId == user.Id))
                {
                    var wantedUserMovie = await _userSavedMovieRepository.GetAsync(x => x.MovieId == movieId &&
                                                                                        x.AppUserId == user.Id);
                    _userSavedMovieRepository.Delete(wantedUserMovie);
                    check = false;
                }
                else
                {
                    await _userSavedMovieRepository.CreateAsync(new UserSavedMovie
                    {
                        MovieId = movieId,
                        AppUserId = user.Id
                    });
                }
                await _userSavedMovieRepository.CommitAsync();
            }
            return check;
        }

        public async Task<List<Core.Models.Movie>> GetUserSavedMoviesAsync(string username)
        {
            var user = await _accountService.GetUserByNameAsync(username);
            if (user is not null)
            {
                var movieIds = await _userSavedMovieRepository.Table.Where(x => x.AppUser.Id == user.Id)
                                                                            .Select(x => x.MovieId).ToListAsync();
                return await _movieService.GetAllAsync(x => movieIds.Contains(x.Id), "MovieImages", "MovieGenres");
            }
            return null;
        }

        public async Task<List<int>> GetUserSavedMoviesIdsAsync(string username)
        {
            var user = await _accountService.GetUserByNameAsync(username);
            if (user is not null)
                return await _userSavedMovieRepository.Table.Where(x => x.AppUserId == user.Id)
                                                                    .Select(x => x.MovieId).ToListAsync();
            return null;
        }

    }
}
