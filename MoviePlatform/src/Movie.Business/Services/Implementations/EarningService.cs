using Movie.Business.Services.Interfaces;
using Movie.Core.Models;
using Movie.Core.Repositories;

namespace Movie.Business.Services.Implementations
{
    public class EarningService : IEarningService
    {
        private readonly IEarningRepository _earningRepository;

        public EarningService(IEarningRepository earningRepository)
        {
            _earningRepository = earningRepository;
        }
        public async Task CreateAsync(int amount, string userId)
        {
            await _earningRepository.CreateAsync(new Earning { Amount = amount, AppUserId = userId });
            await _earningRepository.CommitAsync();
        }
    }
}
