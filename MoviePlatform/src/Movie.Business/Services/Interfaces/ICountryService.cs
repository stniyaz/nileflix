using Movie.Core.Models;

namespace Movie.Business.Services.Interfaces
{
	public interface ICountryService
	{
		Task<List<Country>> GetAllAsync();
	}
}
