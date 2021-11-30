using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Data.DTO;
using DatingApp.API.Entities;
using DatingApp.API.Extensions;
using DatingApp.API.Helpers;
using DatingApp.API.Interface;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository IuserRepository;
        private readonly IMapper mapper;
        private readonly IPhotoService iphotoService;

        public UsersController(IUserRepository IuserRepository, IMapper mapper, IPhotoService iphotoService)
        {
            this.iphotoService = iphotoService;
            this.mapper = mapper;
            this.IuserRepository = IuserRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers([FromQuery]UserParams userParams)
        {
            // var users = await this.IuserRepository.GetUsersAsync();
            // var retVal = this.mapper.Map<IEnumerable<MemberDTO>>(users);
            // return Ok(retVal);
            userParams.CurrentUserName = User.GetUserName();
            var user = await this.IuserRepository.GetUserByUserNameAsync(User.GetUserName());

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = user.Gender == "male" ? "female" : user.Gender;
            }

            var users = await this.IuserRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }

        // GET api/values/5
        // [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberDTO>> GetById(int id)
        {
            var user = await this.IuserRepository.GetUserByIdAsync(id);
            var retVal = this.mapper.Map<MemberDTO>(user);
            return retVal;
        }

        // GET api/values/""NAME""
        [HttpGet("{userName}", Name = "GetUser")]
        public async Task<ActionResult<MemberDTO>> GetByName(string userName)
        {
            // var user = await this.IuserRepository.GetUserByUserNameAsync(userName);
            // var retVal = this.mapper.Map<AppUser,MemberDTO>(user);
            // return retVal;
            return await this.IuserRepository.GetMember(userName);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            var userName = User.GetUserName();
            var user = await this.IuserRepository.GetUserByUserNameAsync(userName);
            this.mapper.Map(memberUpdateDTO, user);
            this.IuserRepository.UpdateUser(user);
            if (await this.IuserRepository.SaveAll())
            {
                return NoContent();
            }
            return BadRequest("Failed to update user");
        }


        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await this.IuserRepository.GetUserByUserNameAsync(User.GetUserName());
            var result = await iphotoService.AddPhotoAsync(file);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            var photo = new Photo()
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };
            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }
            user.Photos.Add(photo);
            if (await this.IuserRepository.SaveAll())
            {
                // return different header response and status 201
                return CreatedAtRoute("GetUser", new { userName = user.UserName }, this.mapper.Map<PhotoDTO>(photo));
            }
            return BadRequest("Problem in adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await this.IuserRepository.GetUserByUserNameAsync(User.GetUserName());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo.IsMain)
            {
                return BadRequest("This is your main photo");
            }
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;
            if (await IuserRepository.SaveAll()) return NoContent();
            return BadRequest("Failed to set the main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await IuserRepository.GetUserByUserNameAsync(User.GetUserName());

            var photo = user.Photos.FirstOrDefault(x=> x.Id == photoId);
            if(photo == null) return NotFound();
            if(photo.IsMain) return BadRequest("Cannot delete main photo");

            if(photo.PublicId !=null){
               var result =  await  iphotoService.DeletePhotoAsync(photo.PublicId);
               if(result.Error != null) return BadRequest(result.Error);
            }
            user.Photos.Remove(photo);
            if(await IuserRepository.SaveAll()) return Ok();
            return BadRequest("Fail to delete photo");


        }




    }
}