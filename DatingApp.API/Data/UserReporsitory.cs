using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.API.Data.DTO;
using DatingApp.API.Entities;
using DatingApp.API.Helpers;
using DatingApp.API.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            .Where(x => x.UserName == userName)
            .ProjectTo<MemberDTO>(imapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        }

        public async Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams)
        {
            var query = dataContext.Users.AsQueryable();
            query = query.Where(x => x.UserName != userParams.CurrentUserName);
            query = query.Where(x => x.Gender == userParams.Gender);
            var minDob = DateTime.Today.AddYears(-userParams.maxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.minAge);
            query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);
            query = userParams.OrderBy switch
            {
                "lastCreated" => query.OrderByDescending(c => c.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };
            return await PagedList<MemberDTO>.CreateAsync(query.ProjectTo<MemberDTO>(imapper.ConfigurationProvider).AsNoTracking()
                , userParams.PageNumber, userParams.PageSize);
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