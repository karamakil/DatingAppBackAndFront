using DatingApp.API.Entities;

namespace DatingApp.API.Interface
{
    public interface ITokenService
    {
         string CreateToken(AppUser appUser);
    }
}