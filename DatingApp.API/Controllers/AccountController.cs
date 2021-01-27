using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Data.DTO;
using DatingApp.API.Entities;
using DatingApp.API.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
            this.tokenService = tokenService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await this.UserExists(registerDTO.UserName)) return BadRequest("user Exists");
            // var user = mapper.Map<AppUser>(registerDTO);
            using (var hash = new HMACSHA512())
            {
                var user = new DatingApp.API.Entities.AppUser()
                {
                    UserName = registerDTO.UserName.ToLower(),
                    KnownAs = registerDTO.KnownAs,
                    Gender = registerDTO.Gender,
                    DateOfBirth = registerDTO.DateOfBirth,
                    City = registerDTO.City,
                    Country = registerDTO.Country,
                    Password = hash.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                    PasswordSalt = hash.Key,
                };
                // user.UserName = registerDTO.UserName.ToLower();
                // user.Password = hash.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
                // user.PasswordSalt = hash.Key;


                this.context.Users.Add(user);
                await context.SaveChangesAsync();

                return new UserDTO()
                {
                    UserName = user.UserName,
                    Token = this.tokenService.CreateToken(user),
                    KnownAs = user.KnownAs,
                };
            }
        }

        private async Task<bool> UserExists(string userName)
        {
            return await this.context.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await this.context.Users.Include("Photos")
            .FirstOrDefaultAsync(x => x.UserName == loginDTO.UserName);

            if (user == null) return Unauthorized("Invaled userName");
            using var hash = new HMACSHA512(user.PasswordSalt);
            var pas = hash.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            for (int i = 0; i < pas.Length; i++)
            {
                if (pas[i] != user.Password[i]) return Unauthorized("Password not equal");
            }

            return new UserDTO()
            {
                UserName = user.UserName,
                Token = this.tokenService.CreateToken(user),
                photoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
            };
        }

    }
}