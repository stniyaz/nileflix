using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Data.DAL;

namespace Movie.Data.Repositories
{
	public class CountryRepository : GenericRepository<Country>, ICountryRepository
	{
		public CountryRepository(AppDbContext context) : base(context)
		{
		}
	}
}
