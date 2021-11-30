using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();


            //user this to seed data
            //var host = CreateHostBuilder(args).Build();
            //using (var scope = host.Services.CreateScope())
            //{
            //    var service = scope.ServiceProvider;

            //    var userManager = service.GetRequiredService<UserManager<AppUser>>();
            //    var roleManager = service.GetRequiredService<RoleManager<AppRole>>();

            //    try
            //    {
            //        var context = service.GetRequiredService<DataContext>();
            //        context.Database.Migrate();
            //         Seed.SeedUsers(userManager, roleManager);
            //    }
            //    catch (System.Exception ex)
            //    {
            //        var logger = service.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "Exception occured in program.cs ");
            //    }
            //    host.Run();
            //}

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
