using Microsoft.EntityFrameworkCore;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;
using Movie.Core.Repositories;
using System.Linq.Expressions;

namespace Movie.Business.Services.Implementations
{
    public class MovieGenreService : IMovieGenreService
    {
        private readonly IMovieGenreRepository _movieGenreRepository;

        public MovieGenreService(IMovieGenreRepository movieGenreRepository)
        {
            _movieGenreRepository = movieGenreRepository;
        }
        public async Task<List<MovieGenre>> GetAllAsync(Expression<Func<MovieGenre, bool>>? expression = null, params string[]? includes)
        {
            return await _movieGenreRepository.GetAllAsync(expression, includes).ToListAsync();
        }
    }
}
