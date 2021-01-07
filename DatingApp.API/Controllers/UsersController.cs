using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Data.DTO;
using DatingApp.API.Entities;
using DatingApp.API.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository IuserRepository;
        private readonly IMapper mapper;

        public UsersController(IUserRepository IuserRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.IuserRepository = IuserRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            // var users = await this.IuserRepository.GetUsersAsync();
            // var retVal = this.mapper.Map<IEnumerable<MemberDTO>>(users);
            // return Ok(retVal);
            var retVal = await this.IuserRepository.GetMembersAsync();
            return Ok(retVal);
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
        // [Authorize]
        [HttpGet("{userName}")]
        public async Task<ActionResult<MemberDTO>> GetByName(string userName)
        {
            // var user = await this.IuserRepository.GetUserByUserNameAsync(userName);
            // var retVal = this.mapper.Map<AppUser,MemberDTO>(user);
            // return retVal;
            return await this.IuserRepository.GetMember(userName);


        }


    }
}