using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.CustomExceptions.GenreExceptions;
using Movie.Business.DTOs.GenreDTOs;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;
using Movie.Core.Repositories;
using System.Linq.Expressions;

namespace Movie.Business.Services.Implementations
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        string folderPath = "uploads/genreImages";
        public GenreService(IGenreRepository genreRepository,
                            IWebHostEnvironment env,
                            IMapper mapper)
        {
            _genreRepository = genreRepository;
            _env = env;
            _mapper = mapper;
        }
        public async Task CreateAsync(GenreCreateDTO dto)
        {
            await CheckName(dto.Name);
            CheckImage(dto.ImageFile);

            var newGenre = _mapper.Map<Genre>(dto);
            newGenre.ImageUrl = Helpers.Common.FileManager.Save(_env.WebRootPath, folderPath, dto.ImageFile);
            await _genreRepository.CreateAsync(newGenre);
            await _genreRepository.CommitAsync();
        }

        public async Task<List<Genre>> GetAllAsync(Expression<Func<Genre, bool>>? expression = null, params string[]? includes)
        {
            return await _genreRepository.GetAllAsync(expression, includes).ToListAsync();
        }

        public async Task<Genre> GetAsync(Expression<Func<Genre, bool>>? expression = null, params string[]? includes)
        {
            return await _genreRepository.GetAsync(expression, includes);
        }

        public IQueryable<Genre> GetQuery(Expression<Func<Genre, bool>>? expression = null, params string[]? includes)
        {
            return _genreRepository.GetAllAsync();
        }

        public async Task HardDeleteAsync(int id)
        {
            var wanted = await _genreRepository.GetAsync(x => x.Id == id);
            if (wanted is null) throw new GenreNotFoundException();

            Helpers.Common.FileManager.Remove(_env.WebRootPath, folderPath, wanted.ImageUrl);
            _genreRepository.Delete(wanted);

            await _genreRepository.CommitAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var wanted = await _genreRepository.GetAsync(x => x.Id == id);
            if (wanted is null) throw new GenreNotFoundException();

            wanted.IsDeleted = !wanted.IsDeleted;
            await _genreRepository.CommitAsync();
        }

        public async Task<List<Genre>> SearchByAsync(string? search)
        {
            var genres = _genreRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(search))
            {
                if (search.Length >= 2)
                    genres = genres.Where(x => x.Name.Trim().ToLower().Contains(search.Trim()));
                else
                    throw new InvalidSearchException();
            }

            return await genres.ToListAsync();
        }

        public async Task UpdateAsync(GenreUpdateDTO dto)
        {
            var exist = await _genreRepository.GetAsync(x => x.Id == dto.Id);
            if (exist is null) throw new GenreNotFoundException();

            await CheckName(dto.Name, exist.Id);

            if (dto.ImageFile is not null)
            {
                CheckImage(dto.ImageFile);
                Helpers.Common.FileManager.Remove(_env.WebRootPath, folderPath, exist.ImageUrl);
                exist.ImageUrl = Helpers.Common.FileManager.Save(_env.WebRootPath, folderPath, dto.ImageFile);
            }

            // trim string properties
            foreach (var property in dto.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string)property.GetValue(dto);
                    if (value != null)
                    {
                        value = value.Trim();
                        property.SetValue(dto, value);
                    }
                }
            }

            exist = _mapper.Map(dto, exist);

            await _genreRepository.CommitAsync();
        }

        private void CheckImage(IFormFile imageFile)
        {
            if (imageFile.Length > 2097152)
                throw new GenreImageLengthException("ImageFile", "Please upload a photo less than 2MB in size.");
            if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
                throw new GenreImageContentTypeException("ImageFile", "Please only upload photo in jpg (jpeg) or png format.");
        }

        private async Task CheckName(string name, int? id = null)
        {
            var genres = await _genreRepository.GetAllAsync().ToListAsync();
            if (genres.Any(x => x.Name == name && x.Id != id))
                throw new ExistGenreException("Name", "This genre already exists.");
        }
    }
}