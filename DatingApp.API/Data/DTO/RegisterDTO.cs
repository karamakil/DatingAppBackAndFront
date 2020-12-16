using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Data.DTO
{
    public class RegisterDTO
    {
        #region Properties

        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8,MinimumLength=4)]
        public string Password { get; set; }

        #endregion
    }
}