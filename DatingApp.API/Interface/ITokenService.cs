using DatingApp.API.Entities;
using System.Threading.Tasks;

namespace DatingApp.API.Interface
{
    public interface ITokenService
    {
         Task<string> CreateToken(AppUser appUser);
    }
}