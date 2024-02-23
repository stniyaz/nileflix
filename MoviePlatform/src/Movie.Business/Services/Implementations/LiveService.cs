using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Movie.Business.CustomExceptions.LiveExceptions;
using Movie.Business.DTOs.LiveDTOs;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;
using Movie.Core.Repositories;
using System.Linq.Expressions;

namespace Movie.Business.Services.Implementations
{
    public class LiveService : ILiveService
    {
        private readonly ILiveRepository _liveRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private string livePath = "uploads/liveImages";
        public LiveService(ILiveRepository liveRepository,
                           IMapper mapper,
                           IWebHostEnvironment env)
        {
            _liveRepository = liveRepository;
            _mapper = mapper;
            _env = env;
        }
        public async Task CreateAsync(LiveCreateDTO live)
        {
            CheckImage(live.ImageFile);

            var newLive = _mapper.Map<Live>(live);
            newLive.ImageUrl = Helpers.Common.FileManager.Save(_env.WebRootPath, livePath, live.ImageFile);
            await _liveRepository.CreateAsync(newLive);
            await _liveRepository.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var wanted = await _liveRepository.GetAsync(x => x.Id == id);
            if (wanted is null) throw new LiveNotFoundException();
            Helpers.Common.FileManager.Remove(_env.WebRootPath, livePath, wanted.ImageUrl);
            _liveRepository.Delete(wanted);
            await _liveRepository.CommitAsync();
        }

        public async Task<List<Live>> GetAllAsync(Expression<Func<Live, bool>>? expression = null, params string[]? includes)
        {
            return await _liveRepository.GetAllAsync(expression, includes).ToListAsync();
        }

        public async Task<Live> GetAsync(Expression<Func<Live, bool>>? expression = null, params string[]? includes)
        {
            return await _liveRepository.GetAsync(expression, includes);
        }

        public async Task SoftDelete(int id)
        {
            var wanted = await _liveRepository.GetAsync(x => x.Id == id);
            if (wanted is null) throw new LiveNotFoundException();
            wanted.IsDeleted = !wanted.IsDeleted;
            await _liveRepository.CommitAsync();
        }

        public async Task UpdateAsync(LiveUpdateDTO live)
        {
            var exist = await _liveRepository.GetAsync(x => x.Id == live.Id);
            if (exist is null) throw new LiveNotFoundException();

            if (live.ImageFile is not null)
            {
                CheckImage(live.ImageFile);
                Helpers.Common.FileManager.Remove(_env.WebRootPath, livePath, exist.ImageUrl);
                exist.ImageUrl = Helpers.Common.FileManager.Save(_env.WebRootPath, livePath, live.ImageFile);
            }

            exist = _mapper.Map(live, exist);
            await _liveRepository.CommitAsync();
        }

        private void CheckImage(IFormFile imageFile)
        {
            if (imageFile.Length > 2097152)
                throw new LiveImageLengthException("ImageFile", "Please upload a photo less than 2MB in size.");
            if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
                throw new LiveContentTypeException("ImageFile", "Please only upload photo in jpg (jpeg) or png format.");
        }
    }
}
