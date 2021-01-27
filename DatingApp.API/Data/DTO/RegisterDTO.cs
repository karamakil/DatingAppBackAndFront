using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Data.DTO
{
    public class RegisterDTO
    {
        #region Properties

        [Required] public string UserName { get; set; }
        [Required] public string KnownAs { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Country { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 1)]
        public string Password { get; set; }

        #endregion
    }
}