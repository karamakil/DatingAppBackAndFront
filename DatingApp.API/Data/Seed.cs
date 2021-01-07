using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DatingApp.API.Entities;

namespace DatingApp.API.Data
{
    public class Seed
    {
        #region Static methods
            
        public static void SeedUsers(DataContext dataContext)
        {
            if (dataContext.Users.Any()) return;

            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var jsonUser = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach (var user in jsonUser)
            {
                using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("1"));
                user.PasswordSalt = hmac.Key;
                dataContext.Users.Add(user);
            }
            dataContext.SaveChanges();

        }

        #endregion

    }
}