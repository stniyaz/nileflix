using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Data.DAL;

namespace Movie.Data.Repositories
{
    public class UserWatchedMovieRepository : GenericRepository<UserWatchedMovie>, IUserWatcedMovieRepository
    {
        public UserWatchedMovieRepository(AppDbContext context) : base(context)
        {
        }
    }
}
