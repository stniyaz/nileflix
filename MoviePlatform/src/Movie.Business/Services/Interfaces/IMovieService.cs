using Movie.Business.DTOs.MovieDTOs;
using Movie.Core.Models;
using System.Linq.Expressions;

namespace Movie.Business.Services.Interfaces
{
    public interface IMovieService
    {
        Task Create(MovieCreateDTO dto);

        Task<List<Core.Models.Movie>> SortByAsync(int? sortBy, string? search);

        Task<Core.Models.Movie> GetAsync(Expression<Func<Core.Models.Movie, bool>>? expression = null, params string[]? includes);
        Task<List<Core.Models.Movie>> GetAllAsync(Expression<Func<Core.Models.Movie, bool>>? expression = null, params string[]? includes);
        IQueryable<Core.Models.Movie> GetQuery(Expression<Func<Core.Models.Movie, bool>>? expression = null, params string[]? includes);
    }
}
