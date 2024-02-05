using Movie.Core.Repositories;
using Movie.Data.DAL;

namespace Movie.Data.Repositories
{
    public class MovieRepository : GenericRepository<Core.Models.Movie>, IMovieRepository
    {
        public MovieRepository(AppDbContext context) : base(context)
        {
        }
    }
}
