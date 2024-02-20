using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Data.DAL;

namespace Movie.Data.Repositories
{
    public class EarningRepository : GenericRepository<Earning>, IEarningRepository
    {
        public EarningRepository(AppDbContext context) : base(context)
        {
        }
    }
}
