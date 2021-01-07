using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Data.DTO;
using DatingApp.API.Entities;

namespace DatingApp.API.Interface
{
    public interface IUserRepository
    {
        #region Methods

        void UpdateUser(AppUser user);
        Task<bool> SaveAll();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUserNameAsync(string userName);

        Task<IEnumerable<MemberDTO>> GetMembersAsync();
        Task<MemberDTO> GetMember(string userName);

        #endregion
    }
}