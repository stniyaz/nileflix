using Microsoft.Extensions.DependencyInjection;
using Movie.Core.Repositories;
using Movie.Data.Repositories;

namespace Movie.Data
{
    public static class ServiceRegistration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IMovieGenreRepository, MovieGenreRepository>();
            services.AddScoped<IMovieImageRepository, MovieImageRepository>();
            services.AddScoped<IUserSavedMovieRepository, UserSavedMovieRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IEarningRepository, EarningRepository>();
            services.AddScoped<ICommentReactionRepository, CommentReactionRepository>();
            services.AddScoped<IUserWatcedMovieRepository, UserWatchedMovieRepository>();
            services.AddScoped<ILiveRepository, LiveRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
        }
    }
}
