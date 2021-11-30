using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.Interface;
using DatingApp.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            services.AddScoped<LogUserActivity>();

            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserReporsitory>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();

            //services.AddDbContext<DataContext>(x =>
            //x.UseSqlServer(config.GetConnectionString("DefaultConnection"))
            //);

            services.AddDbContext<DataContext>(options =>
             options.UseSqlServer(
                 config.GetConnectionString("DefaultConnection"), sqlServerOptionsAction: sqlOptions =>
                 {
                     sqlOptions.EnableRetryOnFailure();
                 }), ServiceLifetime.Transient);


            return services;
        }

    }
}