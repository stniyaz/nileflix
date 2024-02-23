using Movie.Business.DTOs.MovieDTOs;
using System.Linq.Expressions;

namespace Movie.Business.Services.Interfaces
{
    public interface IMovieService
    {
        Task CreateAsync(MovieCreateDTO dto);
        Task SoftDeleteAsync(int id);
        Task UpdateAsync(MovieUpdateDTO dto);
        Task HardDeleteAsync(int id);
        Task<Core.Models.Movie> GetMovieWithAllIncludes(int id);

        Task<bool> CheckVideoAndUser(int id, string username);
        Task<List<Core.Models.Movie>> GetAllHome(int? genreId, string? search);
        Task<List<Core.Models.Movie>> SearchByAsync(string? search);

        Task<Core.Models.Movie> GetAsync(Expression<Func<Core.Models.Movie, bool>>? expression = null, params string[]? includes);
        Task<List<Core.Models.Movie>> GetAllAsync(Expression<Func<Core.Models.Movie, bool>>? expression = null, params string[]? includes);
        IQueryable<Core.Models.Movie> GetQuery(Expression<Func<Core.Models.Movie, bool>>? expression = null, params string[]? includes);
        Task<List<Core.Models.Movie>> GetLatestMoviesAsync();
    }
}
