using System.Text;
using System.Security.Cryptography;
using System.Net;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService )
        {
            this._context = context;
            this._tokenService = tokenService;
        }


        [HttpPost("register")]
        //dto: data transfer objects 

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerdto) {
            
            using var hmac = new HMACSHA512(); // hashing, idisposbale interface, using keyword 

            if (await UserExists(registerdto.Username)) {
                return BadRequest("Username is already taken!");
            }
            var user = new AppUser {
                UserName = registerdto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerdto.Password)),
                PasswordSalt =  hmac.Key
            } ;

            this._context.Users.Add(user);
            await this._context.SaveChangesAsync();

            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        } 


        private async Task<bool> UserExists(string username) {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }



        [HttpPost("login")]

        public async Task<ActionResult<UserDto>> Login(LoginDto logindto) {

            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == logindto.Username);

            if (user == null) return Unauthorized("Invalid username!");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));

            for (int i = 0; i < computedHash.Length; i++) {
                
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password!");
            }


            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }
    }
}