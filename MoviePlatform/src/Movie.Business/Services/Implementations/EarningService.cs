using Microsoft.EntityFrameworkCore;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;
using Movie.Core.Repositories;
using System.Linq.Expressions;

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

        public async Task<List<Earning>> GetAllAsync(Expression<Func<Earning, bool>>? expression, params string[]? includes)
        {
            return await _earningRepository.GetAllAsync(expression, includes).ToListAsync();
        }

        public async Task<double> GetAllEarnedMoneyAsync(Expression<Func<Earning, bool>>? expression, params string[]? includes)
        {
            var result = await _earningRepository.GetAllAsync(expression, includes).Select(x => x.Amount).SumAsync();
            return (double)result / 100;
        }

        public async Task<List<Earning>> GetSearchByAsync(string search)
        {
            var earnings = _earningRepository.GetAllAsync(null, "AppUser");
            if (!string.IsNullOrEmpty(search))
            {
                if (search.Length >= 2)
                    earnings = earnings.Where(x => x.AppUser.FirstName.Trim().ToLower().Contains(search.Trim())
                                                || x.AppUser.LastName.Trim().ToLower().Contains(search.Trim())
                                                || x.AppUser.Email.Trim().ToLower().Contains(search.Trim())
                                                || x.AppUser.UserName.Trim().ToLower().Contains(search.Trim()));
                else
                    throw new InvalidSearchException();
            }
            return await earnings.ToListAsync();
        }
    }
}
