using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.API.Data.DTO;
using DatingApp.API.Entities;
using DatingApp.API.Interface;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class UserReporsitory : IUserRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper imapper;

        #region constructor
        public UserReporsitory(DataContext dataContext, IMapper imapper)
        {
            this.dataContext = dataContext;
            this.imapper = imapper;
        }


        #endregion

        #region PublicMethods

        public async Task<MemberDTO> GetMember(string userName)
        {
            return await dataContext.Users
            .Where(x=> x.UserName == userName)
            .ProjectTo<MemberDTO>(imapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
            
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await dataContext.Users
            .ProjectTo<MemberDTO>(imapper.ConfigurationProvider)
            .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await this.dataContext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string userName)
        {
            return await this.dataContext.Users.
            Include(x => x.Photos).
            SingleOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await this.dataContext.Users.Include(x => x.Photos).ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await this.dataContext.SaveChangesAsync() > 0;
        }

        public void UpdateUser(AppUser user)
        {
            this.dataContext.Entry(user).State = EntityState.Modified;
        }

        #endregion
    }
}