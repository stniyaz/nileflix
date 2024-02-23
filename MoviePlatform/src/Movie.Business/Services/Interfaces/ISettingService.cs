using Movie.Business.DTOs.SettingDTOs;
using Movie.Core.Models;
using System.Linq.Expressions;

namespace Movie.Business.Services.Interfaces
{
    public interface ISettingService
    {
        Task<List<Setting>> GetAllAsync(Expression<Func<Setting, bool>>? expression = null, params string[]? includes);
        Task<Setting> GetAsync(Expression<Func<Setting, bool>>? expression = null, params string[]? includes);
        Task<List<Setting>> SearchByAsync(string search);
        Task Update(SettingUpdateDTO dto);
    }
}
