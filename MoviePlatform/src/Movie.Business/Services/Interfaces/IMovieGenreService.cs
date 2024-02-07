using Movie.Core.Models;
using System.Linq.Expressions;

namespace Movie.Business.Services.Interfaces
{
    public interface IMovieGenreService
    {
        Task<List<MovieGenre>> GetAllAsync(Expression<Func<MovieGenre,bool>>? expression = null, params string[]? includes);
    }
}
