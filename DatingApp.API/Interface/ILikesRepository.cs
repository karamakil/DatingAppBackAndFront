using DatingApp.API.Data.DTO;
using DatingApp.API.Entities;
using DatingApp.API.Helpers;
using System.Threading.Tasks;

namespace DatingApp.API.Interface
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
        Task<AppUser> GetUserWithLike(int userId);
        Task<PagedList<LikeDTO>> GetUserLikes(LikesParams likesParams);

    }
}
