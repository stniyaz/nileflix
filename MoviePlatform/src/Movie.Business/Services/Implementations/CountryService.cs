using Microsoft.EntityFrameworkCore;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;
using Movie.Core.Repositories;

namespace Movie.Business.Services.Implementations
{
	public class CountryService : ICountryService
	{
		private readonly ICountryRepository _countryRepository;

		public CountryService(ICountryRepository countryRepository)
        {
			_countryRepository = countryRepository;
		}
        public async Task<List<Country>> GetAllAsync()
		{
			return await _countryRepository.GetAllAsync().ToListAsync();
		}
	}
}
