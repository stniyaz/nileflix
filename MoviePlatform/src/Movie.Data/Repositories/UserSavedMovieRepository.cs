using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Data.DAL;

namespace Movie.Data.Repositories
{
    public class UserSavedMovieRepository : GenericRepository<UserSavedMovie>, IUserSavedMovieRepository
    {
        public UserSavedMovieRepository(AppDbContext context) : base(context)
        {
        }
    }
}
