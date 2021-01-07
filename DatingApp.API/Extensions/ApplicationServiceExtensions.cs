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
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository,UserReporsitory>();
            services.AddDbContext<DataContext>(x =>
            x.UseSqlServer(config.GetConnectionString("DefaultConnection"))
            );
            return services;
        }

    }
}