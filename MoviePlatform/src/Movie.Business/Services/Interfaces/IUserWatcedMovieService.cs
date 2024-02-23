using Movie.Core.Models;
using System.Linq.Expressions;

namespace Movie.Business.Services.Interfaces
{
    public interface IUserWatcedMovieService
    {
        Task ViewCounterAsync(int movieId);
        Task<List<UserWatchedMovie>> GetAllAsync(Expression<Func<UserWatchedMovie, bool>>? expression = null,
                                                 params string[]? includes);
    }
}
