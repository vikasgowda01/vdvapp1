using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using API.entites;
using API.data;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using AutoMapper;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenServices _TokenService;
        private readonly IMapper _mapper;
        public AccountController(DataContext context, ITokenServices TokenService, IMapper mapper){
           
            _mapper = mapper;
            _TokenService = TokenService;
            _context = context;

        }
        [HttpPost("register")] //Post: api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.username)) return BadRequest("user is taken");
            
            var user =_mapper.Map<AppUser>(registerDto);
            using var hmac = new HMACSHA512();//hash salt assign random value to password and use hashing algoirthm
            //keyword using class removed from memory -( garbage collection)  is used because if any unused class is present it will be disposed so we this dispose is done by using keyword.
            
            
                user.UserName =registerDto.username.ToLower();
                user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password));
                user.PasswordSalt=hmac.Key;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                username =user.UserName,
                Token = _TokenService.CreateToken(user),
                KnownAs = user.KnownAs
                
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user= await _context.Users
            .Include(p=>p.Photos)
            .SingleOrDefaultAsync(x => x.UserName ==loginDto.username);
            if (user == null) return Unauthorized("Invalid username");
            //password check
            using var hmac = new HMACSHA512(user.PasswordSalt);//algorithm to check password salt
            var ComputeHash =hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

            // apply for loop to check each element of a password in byte
            for(int i=0; i< ComputeHash.Length; i++){
                if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }
            return new UserDto
            {
                username =user.UserName,
                Token = _TokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x=> x.IsMain)?.Url,
                KnownAs =user.KnownAs
            };

        }
    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(x =>x.UserName==username.ToLower());
    }

    }

}