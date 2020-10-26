using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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

        public AccountController(DataContext context, ITokenService tokenService)
        {
            this.context = context;
            this.tokenService = tokenService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await this.UserExists(registerDTO.UserName)) return BadRequest("user Exists");

            using (var hash = new HMACSHA512())
            {
                var user = new AppUser()
                {
                    UserName = registerDTO.UserName.ToLower(),
                    Password = hash.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                    PasswordSalt = hash.Key
                };
                this.context.Users.Add(user);
                await context.SaveChangesAsync();

                return new UserDTO()
                {
                    UserName = user.UserName,
                    Token = this.tokenService.CreateToken(user)
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
            var user = await this.context.Users.
            FirstOrDefaultAsync(x => x.UserName == loginDTO.UserName);
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
                Token = this.tokenService.CreateToken(user)
            };
        }

    }
}