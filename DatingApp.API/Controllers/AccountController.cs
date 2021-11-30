using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Data.DTO;
using DatingApp.API.Entities;
using DatingApp.API.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await this.UserExists(registerDTO.UserName)) return BadRequest("user Exists");

            // var user = mapper.Map<AppUser>(registerDTO);
            var user = new DatingApp.API.Entities.AppUser()
            {
                UserName = registerDTO.UserName.ToLower(),
                KnownAs = registerDTO.KnownAs,
                Gender = registerDTO.Gender,
                DateOfBirth = registerDTO.DateOfBirth,
                City = registerDTO.City,
                Country = registerDTO.Country,
            };

            var result = await this.userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDTO()
            {
                UserName = user.UserName,
                Token = await this.tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }

        private async Task<bool> UserExists(string userName)
        {
            return await this.userManager.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await this.userManager.Users.Include("Photos")
            .FirstOrDefaultAsync(x => x.UserName == loginDTO.UserName.ToLower());

            if (user == null) return Unauthorized("Invaled userName");
            var result = await this.signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded) return Unauthorized();

            return new UserDTO()
            {
                UserName = user.UserName,
                Token = await this.tokenService.CreateToken(user),
                photoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }

    }
}