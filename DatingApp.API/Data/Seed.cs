using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DatingApp.API.Entities;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Data
{
    public class Seed
    {
        #region Static methods

        public static async Task SeedUsers(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {

            if (userManager.Users.Any()) return;

            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var jsonUser = JsonSerializer.Deserialize<List<AppUser>>(userData);

            var roles = new List<AppRole>()
            {
                new AppRole(){Name = "Member"},
                new AppRole(){Name = "Admin"},
                new AppRole(){Name = "Moderator"}
            };

            foreach (var item in roles)
            {
                await roleManager.CreateAsync(item);
            }


            foreach (var user in jsonUser)
            {
                //using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                //user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("1"));
                //user.PasswordSalt = hmac.Key;
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser() { UserName = "Admin" };
            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRoleAsync(admin, "Admin");
            await userManager.AddToRoleAsync(admin, "Moderator");

            //dataContext.SaveChanges();

        }

        #endregion

    }
}