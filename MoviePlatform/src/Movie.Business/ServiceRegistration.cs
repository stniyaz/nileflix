using Microsoft.Extensions.DependencyInjection;
using Movie.Business.Mapper;
using Movie.Business.Services.Implementations;
using Movie.Business.Services.Interfaces;

namespace Movie.Business
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IGenreService, GenreService>();
            services.AddAutoMapper(typeof(MapProfile).Assembly);
        }
    }
}
