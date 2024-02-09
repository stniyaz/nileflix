using Movie.Business.DTOs.GenreDTOs;
using Movie.Business.Helpers.Pagination;
using Movie.Core.Models;
using System.Linq.Expressions;

namespace Movie.Business.Services.Interfaces
{
    public interface IGenreService
    {
        Task CreateAsync(GenreCreateDTO dto);
        Task UpdateAsync(GenreUpdateDTO dto);
        Task HardDeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        PaginatedList<Genre> SortBy(int? sortBy, string? search, string page);

        Task<Genre> GetAsync(Expression<Func<Genre, bool>>? expression = null, params string[]? includes);
        Task<List<Genre>> GetAllAsync(Expression<Func<Genre, bool>>? expression = null, params string[]? includes);
        IQueryable<Genre> GetQuery(Expression<Func<Genre, bool>>? expression = null, params string[]? includes);
    }
}