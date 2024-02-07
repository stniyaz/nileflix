using Microsoft.EntityFrameworkCore;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;
using Movie.Core.Repositories;
using System.Linq.Expressions;

namespace Movie.Business.Services.Implementations
{
    public class MovieImageService : IMovieImageService
    {
        private readonly IMovieImageRepository _movieImageRepository;

        public MovieImageService(IMovieImageRepository movieImageRepository)
        {
            _movieImageRepository = movieImageRepository;
        }
        public async Task<List<MovieImage>> 
            GetAllAsync(Expression<Func<MovieImage, bool>>? expression = null, params string[]? includes)
        {
            return await _movieImageRepository.GetAllAsync(expression, includes).ToListAsync();
        }
    }
}
