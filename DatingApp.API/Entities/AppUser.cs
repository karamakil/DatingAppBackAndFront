using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DatingApp.API.Entities
{
    public partial class AppUser : IdentityUser<int>
    {

       #region Properties
           
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }   
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }=DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<UserLike> LikedByUsers { get; set; }
        public ICollection<UserLike> LikedUsers { get; set; }

        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessageRecieved { get; set; }
        public ICollection<AppUserRole> AppUserRoles { get; set; }

        #endregion

        #region Public Methods

        //    public int GetAge()
        //    {
        //        return this.DateOfBirth.CalculateAge();
        //    }

        #endregion

    }
}