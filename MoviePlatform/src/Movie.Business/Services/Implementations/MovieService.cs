using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Movie.Business.CustomExceptions.MoiveExceptions;
using Movie.Business.DTOs.MovieDTOs;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;
using Movie.Core.Repositories;
using System.Linq.Expressions;

namespace Movie.Business.Services.Implementations
{
	public class MovieService : IMovieService
	{
		private readonly IMovieRepository _movieRepository;
		private readonly ICountryRepository _countryRepository;
		private readonly IGenreRepository _genreRepository;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;
		private readonly IMovieImageRepository _movieImageRepository;
		private readonly IMovieGenreRepository _movieGenreRepository;
		private string imagePath = "uploads/MovieImages";
		private string videoPath = "uploads/MovieVideos";
		public MovieService(IMovieRepository movieRepository,
							ICountryRepository countryRepository,
							IGenreRepository genreRepository,
							IMapper mapper,
							IWebHostEnvironment env,
							IMovieImageRepository movieImageRepository,
							IMovieGenreRepository movieGenreRepository)
		{
			_movieRepository = movieRepository;
			_countryRepository = countryRepository;
			_genreRepository = genreRepository;
			_mapper = mapper;
			_env = env;
			_movieImageRepository = movieImageRepository;
			_movieGenreRepository = movieGenreRepository;
		}
		public async Task Create(MovieCreateDTO dto)
		{
			// Check
			if (!_countryRepository.Table.Any(x => x.Id == dto.CountryId))
				throw new InvalidCountryIdException("CountryId", "Country not found!");

			if (dto.GenreIds is not null)
			{
				foreach (var genreId in dto.GenreIds)
				{
					if (!_genreRepository.Table.Any(x => x.Id == genreId))
						throw new InvalidGenreIdException();
				}
			}
			CheckImage(dto.CoverImage, "CoverImage");

			if (dto.OtherImages is not null)
			{
				foreach (var img in dto.OtherImages)
				{
					CheckImage(img, "OtherImages");
				}
			}

			if (dto.TrailerVideo is not null)
				CheckVideo(dto.TrailerVideo, "TrailerVideo");

			if (dto.FullVideo is not null)
				CheckVideo(dto.FullVideo, "TrailerVideo");

			// Create
			var newMovie = _mapper.Map<Core.Models.Movie>(dto);
			// Image
			if (dto.CoverImage is not null)
			{
				MovieImage movieImage = new MovieImage
				{
					Movie = newMovie,
					ImageUrl = Helpers.Common.FileManager.Save(_env.WebRootPath, imagePath, dto.CoverImage),
					IsCover = true
				};
				await _movieImageRepository.CreateAsync(movieImage);
			}
			if (dto.OtherImages is not null)
			{
				foreach (var img in dto.OtherImages)
				{
					MovieImage movieImage = new MovieImage
					{
						Movie = newMovie,
						ImageUrl = Helpers.Common.FileManager.Save(_env.WebRootPath, imagePath, img),
						IsCover = false
					};
					await _movieImageRepository.CreateAsync(movieImage);
				}
			}
			// Genre
			if (dto.GenreIds is not null)
			{
				foreach (var id in dto.GenreIds)
				{
					MovieGenre movieGenre = new MovieGenre
					{
						Movie = newMovie,
						GenreId = id,
					};
					await _movieGenreRepository.CreateAsync(movieGenre);
				}
			}
			// Video

			if(dto.TrailerVideo is not null)
			{
				newMovie.TrailerUrl = Helpers.Common.FileManager.Save(_env.WebRootPath,videoPath, dto.TrailerVideo);
			}

            if (dto.FullVideo is not null)
            {
                newMovie.MovieUrl = Helpers.Common.FileManager.Save(_env.WebRootPath, videoPath, dto.FullVideo);
            }

            await _movieRepository.CreateAsync(newMovie);
			await _movieRepository.CommitAsync();
		}

		public async Task<List<Core.Models.Movie>> GetAllAsync(Expression<Func<Core.Models.Movie, bool>>? expression = null, params string[]? includes)
		{
			return await _movieRepository.GetAllAsync(expression, includes).ToListAsync();
		}

		public Task<Core.Models.Movie> GetAsync(Expression<Func<Core.Models.Movie, bool>>? expression = null, params string[]? includes)
		{
			throw new NotImplementedException();
		}

		public IQueryable<Core.Models.Movie> GetQuery(Expression<Func<Core.Models.Movie, bool>>? expression = null, params string[]? includes)
		{
			throw new NotImplementedException();
		}

		public Task<List<Core.Models.Movie>> SortByAsync(int? sortBy, string? search)
		{
			throw new NotImplementedException();
		}

		private void CheckImage(IFormFile imageFile, string propertyName)
		{
			if (imageFile.Length > 2097152)
				throw new MovieImageLengthException(propertyName, "Please upload a photo less than 2MB in size.");
			if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
				throw new MovieImageContentTypeException(propertyName, "Please only upload photo in jpg (jpeg) or png format.");
		}

		private void CheckVideo(IFormFile videoFile, string propertyName)
		{
			if (videoFile.ContentType != "video/x-msvideo" &&
			   videoFile.ContentType != "video/mp4" &&
			   videoFile.ContentType != "video/mpeg" &&
			   videoFile.ContentType != "video/3gpp")
				throw new MovieVideoContentTypeException(propertyName, "Please only upload video in avi, mp4, mpeg or 3gp format.");
		}
	}
}
