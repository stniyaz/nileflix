using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.CustomExceptions.SettingExceptions;
using Movie.Business.DTOs.SettingDTOs;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;
using Movie.Core.Repositories;
using System.Linq.Expressions;

namespace Movie.Business.Services.Implementations
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IMapper _mapper;

        public SettingService(ISettingRepository settingRepository,
                              IMapper mapper)
        {
            _settingRepository = settingRepository;
            _mapper = mapper;
        }
        public async Task<List<Setting>> GetAllAsync(Expression<Func<Setting, bool>>? expression = null, params string[]? includes)
        {
            return await _settingRepository.GetAllAsync(expression, includes).ToListAsync();
        }

        public async Task<Setting> GetAsync(Expression<Func<Setting, bool>>? expression = null, params string[]? includes)
        {
            return await _settingRepository.GetAsync(expression, includes);
        }

        public async Task<List<Setting>> SearchByAsync(string search)
        {
            var settings = _settingRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(search))
            {
                if (search.Length >= 2)
                    settings = settings.Where(x => x.Key.Trim().ToLower().Contains(search.Trim())
                                                || x.Value.Trim().ToLower().Contains(search.Trim()));
                else
                    throw new InvalidSearchException();
            }
            return await settings.ToListAsync();
        }

        public async Task Update(SettingUpdateDTO dto)
        {
            var exist = await _settingRepository.GetAsync(x => x.Id == dto.Id);
            if (exist is null) throw new SettingNotFoundException();

            exist = _mapper.Map(dto, exist);
            await _settingRepository.CommitAsync();
        }
    }
}
