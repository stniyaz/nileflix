using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Data.DAL;

namespace Movie.Data.Repositories
{
	public class MovieImageRepository : GenericRepository<MovieImage>, IMovieImageRepository
	{
		public MovieImageRepository(AppDbContext context) : base(context)
		{
		}
	}
}
