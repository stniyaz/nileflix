using Movie.Core.Models;
using System.Linq.Expressions;

namespace Movie.Business.Services.Interfaces
{
    public interface IEarningService
    {
        Task CreateAsync(int amount, string userId);
        Task<List<Earning>> GetAllAsync(Expression<Func<Earning, bool>>? expression, params string[]? includes);
        Task<double> GetAllEarnedMoneyAsync(Expression<Func<Earning, bool>>? expression, params string[]? includes);
        Task<List<Earning>> GetSearchByAsync(string search);
    }
}
