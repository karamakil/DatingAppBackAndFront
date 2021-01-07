using System;
using System.Collections.Generic;
using DatingApp.API.Extensions;

namespace DatingApp.API.Entities
{
    public partial class AppUser
    {

       #region Properties
           
        public int Id { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }

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

       #endregion

       #region Public Methods

    //    public int GetAge()
    //    {
    //        return this.DateOfBirth.CalculateAge();
    //    }
           
       #endregion
        
    }
}