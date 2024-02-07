using Movie.Core.Models;
using System.Linq.Expressions;

namespace Movie.Business.Services.Interfaces
{
    public interface IMovieImageService
    {
        Task<List<MovieImage>>
            GetAllAsync(Expression<Func<MovieImage, bool>>? expression = null, params string[]? includes);
    }
}
