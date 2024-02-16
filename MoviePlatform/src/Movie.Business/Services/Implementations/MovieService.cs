using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.CustomExceptions.MoiveExceptions;
using Movie.Business.CustomExceptions.UserException;
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
        private readonly IAccountService _accountService;
        private string imagePath = "uploads/movieImages";
        private string videoPath = "uploads/movieVideos";
        private string subPath = "uploads/movieSubs";
        public MovieService(IMovieRepository movieRepository,
                            ICountryRepository countryRepository,
                            IGenreRepository genreRepository,
                            IMapper mapper,
                            IWebHostEnvironment env,
                            IMovieImageRepository movieImageRepository,
                            IMovieGenreRepository movieGenreRepository,
                            IAccountService accountService)
        {
            _movieRepository = movieRepository;
            _countryRepository = countryRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
            _env = env;
            _movieImageRepository = movieImageRepository;
            _movieGenreRepository = movieGenreRepository;
            _accountService = accountService;
        }
        public async Task CreateAsync(MovieCreateDTO dto)
        {
            // Check

            // country
            if (!_countryRepository.Table.Any(x => x.Id == dto.CountryId))
                throw new InvalidCountryIdException("CountryId", "Country not found!");
            // genre
            if (dto.GenreIds is not null)
            {
                foreach (var genreId in dto.GenreIds)
                {
                    if (!_genreRepository.Table.Any(x => x.Id == genreId))
                        throw new InvalidGenreIdException();
                }
            }
            // images
            CheckImage(dto.CoverImage, "CoverImage");

            if (dto.OtherImages is not null)
            {
                foreach (var img in dto.OtherImages)
                {
                    CheckImage(img, "OtherImages");
                }
            }
            // videos
            if (dto.Movie1080 is not null)
                CheckVideo(dto.Movie1080, "Movie1080");

            if (dto.Movie480 is not null)
                CheckVideo(dto.Movie480, "Movie480");

            // subtitle
            if (dto.SubtitleFile is not null)
                CheckSubtitle(dto.SubtitleFile, "SubtitleFile");

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

            if (dto.Movie1080 is not null)
            {
                newMovie.Movie1080pUrl = Helpers.Common.FileManager.Save(_env.WebRootPath, videoPath, dto.Movie1080);
            }

            if (dto.Movie480 is not null)
            {
                newMovie.Movie480pUrl = Helpers.Common.FileManager.Save(_env.WebRootPath, videoPath, dto.Movie480);
            }
            // subtitle
            if (dto.SubtitleFile is not null)
            {
                newMovie.Subtitle = Helpers.Common.FileManager.Save(_env.WebRootPath, subPath, dto.SubtitleFile);
            }

            // Create, Save

            await _movieRepository.CreateAsync(newMovie);
            await _movieRepository.CommitAsync();
        }

        public async Task<List<Core.Models.Movie>> GetAllAsync(Expression<Func<Core.Models.Movie, bool>>? expression = null, params string[]? includes)
        {
            return await _movieRepository.GetAllAsync(expression, includes).ToListAsync();
        }

        public async Task<List<Core.Models.Movie>> GetAllHome(int? genreId, string? search)
        {
            var movies = _movieRepository.GetAllAsync(x => x.IsDeleted == false, "MovieImages", "MovieGenres");

            if (genreId is not null && genreId != 0)
            {
                var genres = _genreRepository.GetAllAsync();
                if (!genres.Any(x => x.Id == genreId))
                    throw new InvalidGenreIdException();
                movies = movies.Where(x => x.MovieGenres.Where(x => x.GenreId == genreId).Any());
            }

            if (!string.IsNullOrEmpty(search))
            {
                if (search.Length >= 2)
                    movies = movies.Where(x => x.Title.Trim().ToLower().Contains(search.Trim().ToLower())
                                            || x.Description.Trim().ToLower().Contains(search.Trim().ToLower()));
                else
                    throw new InvalidSearchException();
            }

            return await movies.ToListAsync();
        }

        public async Task<Core.Models.Movie> GetAsync(Expression<Func<Core.Models.Movie, bool>>? expression = null, params string[]? includes)
        {
            return await _movieRepository.GetAsync(expression, includes);
        }

        public IQueryable<Core.Models.Movie> GetQuery(Expression<Func<Core.Models.Movie, bool>>? expression = null, params string[]? includes)
        {
            throw new NotImplementedException();
        }

        public async Task HardDeleteAsync(int id)
        {
            var wanted = await _movieRepository.GetAsync(x => x.Id == id, "MovieImages");
            if (wanted is null)
                throw new MovieNotFoundException();
            // remove images
            foreach (var img in wanted.MovieImages)
            {
                Helpers.Common.FileManager.Remove(_env.WebRootPath, imagePath, img.ImageUrl);
            }
            // remove sub
            Helpers.Common.FileManager.Remove(_env.WebRootPath, subPath, wanted.Subtitle);
            // remove videos
            Helpers.Common.FileManager.Remove(_env.WebRootPath, videoPath, wanted.Movie1080pUrl);
            Helpers.Common.FileManager.Remove(_env.WebRootPath, videoPath, wanted.Movie480pUrl);

            // remove and save
            _movieRepository.Delete(wanted);
            await _movieRepository.CommitAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var wanted = await _movieRepository.GetAsync(x => x.Id == id);
            if (wanted is null) throw new MovieNotFoundException();
            wanted.IsDeleted = !wanted.IsDeleted;

            await _movieRepository.CommitAsync();
        }

        public Task<List<Core.Models.Movie>> SearchByAsync(string? search)
        {
            var movies = _movieRepository.GetAllAsync(null, "MovieGenres", "MovieImages");

            if (!string.IsNullOrEmpty(search))
            {
                if (search.Length >= 2)
                    movies = movies.Where(x => x.Title.Trim().ToLower().Contains(search.Trim())
                                            && x.Description.Trim().ToLower().Contains(search.Trim()));
                else
                    throw new InvalidSearchException();
            }

            return movies.ToListAsync();
        }

        public async Task UpdateAsync(MovieUpdateDTO dto)
        {
            var existMovie = await _movieRepository.GetAsync(x => x.Id == dto.Id, "MovieImages", "MovieGenres");
            if (existMovie is null) throw new MovieNotFoundException();

            // Check
            // country
            if (!_countryRepository.Table.Any(x => x.Id == dto.CountryId))
                throw new InvalidCountryIdException("CountryId", "Country not found!");
            // genre
            if (dto.GenreIds is not null)
            {
                foreach (var genreId in dto.GenreIds)
                {
                    if (!_genreRepository.Table.Any(x => x.Id == genreId))
                        throw new InvalidGenreIdException();
                }
            }
            // images
            if (dto.CoverImage is not null)
                CheckImage(dto.CoverImage, "CoverImage");

            if (dto.OtherImages is not null)
            {
                foreach (var img in dto.OtherImages)
                {
                    CheckImage(img, "OtherImages");
                }
            }
            // videos
            if (dto.Movie1080 is not null)
                CheckVideo(dto.Movie1080, "Movie1080");
            if (dto.Movie480 is not null)
                CheckVideo(dto.Movie480, "Movie480");
            // subtitle
            if (dto.SubtitleFile is not null)
                CheckSubtitle(dto.SubtitleFile, "SubtitleFile");

            // Update

            // Image

            foreach (var img in existMovie.MovieImages)
            {
                if (!dto.ImageIds.Contains(img.Id) && img.IsCover == false)
                {
                    Helpers.Common.FileManager.Remove(_env.WebRootPath, imagePath, img.ImageUrl);
                }
            }

            existMovie.MovieImages.RemoveAll(mi => !dto.ImageIds.Contains(mi.Id) && mi.IsCover == false);

            if (dto.CoverImage is not null)
            {
                var existCoverImage = existMovie.MovieImages.FirstOrDefault(x => x.IsCover == true);
                Helpers.Common.FileManager.Remove(_env.WebRootPath, imagePath, existCoverImage.ImageUrl);
                existMovie.MovieImages.Remove(existCoverImage);
                MovieImage movieImage = new MovieImage
                {
                    Movie = existMovie,
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
                        Movie = existMovie,
                        ImageUrl = Helpers.Common.FileManager.Save(_env.WebRootPath, imagePath, img),
                        IsCover = false
                    };
                    await _movieImageRepository.CreateAsync(movieImage);
                }
            }

            // genre
            existMovie.MovieGenres.RemoveAll(g => !dto.GenreIds.Contains(g.GenreId));

            if (dto.GenreIds is not null)
            {
                foreach (var id in dto.GenreIds.Where(x => !existMovie.MovieGenres.Any(g => g.GenreId == x)))
                {
                    MovieGenre movieGenre = new MovieGenre
                    {
                        Movie = existMovie,
                        GenreId = id,
                    };
                    await _movieGenreRepository.CreateAsync(movieGenre);
                }
            }
            // video
            if (dto.Movie1080 is not null)
            {
                Helpers.Common.FileManager.Remove(_env.WebRootPath, videoPath, existMovie.Movie1080pUrl);
                existMovie.Movie1080pUrl = Helpers.Common.FileManager.Save(_env.WebRootPath, videoPath, dto.Movie1080);
            }
            if (dto.Movie480 is not null)
            {
                Helpers.Common.FileManager.Remove(_env.WebRootPath, videoPath, existMovie.Movie480pUrl);
                existMovie.Movie480pUrl = Helpers.Common.FileManager.Save(_env.WebRootPath, videoPath, dto.Movie480);
            }
            // subtitle
            if (dto.SubtitleFile is not null)
            {
                Helpers.Common.FileManager.Remove(_env.WebRootPath, subPath, existMovie.Subtitle);
                existMovie.Subtitle = Helpers.Common.FileManager.Save(_env.WebRootPath, subPath, dto.SubtitleFile);
            }

            // map and save
            existMovie = _mapper.Map(dto, existMovie);
            await _movieRepository.CommitAsync();
        }

        public async Task<bool> CheckVideoAndUser(int id, string username)
        {
            var movie = await _movieRepository.GetAsync(x => x.Id == id, "MovieImages", "MovieGenres");
            if (movie is null)
                throw new MovieNotFoundException();

            var user = await _accountService.GetUserByNameAsync(username);

            if (user is null)
                throw new UserNotFoundException();

            var check = user.IsPremium;

            return check;
        }

        public async Task<Core.Models.Movie> GetMovieWithAllIncludes(int id)
        {
            return await _movieRepository.GetAsync(x => x.Id == id, "MovieImages", "MovieGenres");
        }


        // Methods
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
               videoFile.ContentType != "video/3gpp" &&
               videoFile.ContentType != "video/x-matroska")
                throw new MovieVideoContentTypeException(propertyName, "Please only upload video in avi, mp4, mpeg or 3gp format.");
        }

        private void CheckSubtitle(IFormFile subtitleFile, string propertyName)
        {
            var extension = Path.GetExtension(subtitleFile.FileName);
            if (extension.ToLower().Trim() != ".vtt" && extension.ToLower().Trim() != ".srt")
                throw new InvalidSubtitileContentTypeException(propertyName, "Please upload subtitle only vtt or srt format.");
        }

    }
}
