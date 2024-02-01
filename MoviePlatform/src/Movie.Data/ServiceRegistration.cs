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
        }
    }
}
