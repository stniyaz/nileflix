using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movie.Business.Helpers.Mail;
using Movie.Business.Mapper;
using Movie.Business.Services.Implementations;
using Movie.Business.Services.Interfaces;
using Movie.Business.Services.ViewServices;

namespace Movie.Business
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddScoped<IGenreService, GenreService>();
            services.AddAutoMapper(typeof(MapProfile).Assembly);
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IMovieGenreService, MovieGenreService>();
            services.AddScoped<IMovieImageService, MovieImageService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ManageLayoutService>();
            services.AddScoped<IEmailService, EmailService>();

            services.ConfigureApplicationCookie(opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromHours(28);
            });

            services.Configure<IdentityOptions>(opt =>
            {
                opt.SignIn.RequireConfirmedEmail = true;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(241);
            });

            var emailConfig = builder.Configuration.GetSection("EmailConfirmation")
                                                   .Get<EmailConfiguration>();
            builder.Services.AddSingleton(emailConfig);

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromMinutes(15);
            });

        }
    }
}
