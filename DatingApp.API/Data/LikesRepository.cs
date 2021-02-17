using DatingApp.API.Data.DTO;
using DatingApp.API.Entities;
using DatingApp.API.Helpers;
using DatingApp.API.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public class LikesRepository : ILikesRepository
    {
        public DataContext Context { get; }

        public LikesRepository(DataContext context)
        {
            Context = context;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await this.Context.UserLikes.FindAsync(sourceUserId, likedUserId);
        }

        public async Task<PagedList<LikeDTO>> GetUserLikes(LikesParams likesParams)
        {
            var users = this.Context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = this.Context.UserLikes.AsQueryable();
            if (likesParams.Predicate == "liked")
            {
                likes = likes.Where(l => l.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }
            if (likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(l => l.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            var likedUsers = users.Select(u => new LikeDTO()
            {
                Id = u.Id,
                UserName = u.UserName,
                //Age = u.age
                KnownAs = u.KnownAs,
                PhotoUrl = u.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = u.City,
            });

            return await PagedList<LikeDTO>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLike(int userId)
        {
            return await this.Context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
