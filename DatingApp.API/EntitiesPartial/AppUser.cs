using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Data;

namespace DatingApp.API.Entities
{
    public partial class AppUser
    {
        public List<AppUser> GetAll()
        {
            using (var ctx = new DataContext())
            {
                return ctx.Users.ToList();
            }
            
        }
    }
}