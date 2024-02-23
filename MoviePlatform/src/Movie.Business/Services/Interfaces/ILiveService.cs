using Movie.Business.DTOs.LiveDTOs;
using Movie.Core.Models;
using System.Linq.Expressions;

namespace Movie.Business.Services.Interfaces
{
    public interface ILiveService
    {
        Task CreateAsync(LiveCreateDTO live);
        Task UpdateAsync(LiveUpdateDTO live);
        Task SoftDelete(int id);
        Task DeleteAsync(int id);
        Task<Live> GetAsync(Expression<Func<Live, bool>>? expression = null, params string[]? includes);
        Task<List<Live>> GetAllAsync(Expression<Func<Live, bool>>? expression = null, params string[]? includes);
    }
}
