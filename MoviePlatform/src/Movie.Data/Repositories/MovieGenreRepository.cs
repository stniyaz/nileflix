using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Data.DAL;

namespace Movie.Data.Repositories
{
	public class MovieGenreRepository : GenericRepository<MovieGenre>, IMovieGenreRepository
	{
		public MovieGenreRepository(AppDbContext context) : base(context)
		{
		}
	}
}
