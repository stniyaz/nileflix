using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Data.DAL;

namespace Movie.Data.Repositories
{
    public class LiveRepository : GenericRepository<Live>, ILiveRepository
    {
        public LiveRepository(AppDbContext context) : base(context)
        {
        }
    }
}
