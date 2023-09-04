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
using Microsoft.AspNetCore.Identity;


namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
       
        private readonly ITokenServices _TokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager, ITokenServices TokenService, IMapper mapper){
            _userManager = userManager;
           
            _mapper = mapper;
            _TokenService = TokenService;
           

        }
        [HttpPost("register")] //Post: api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.username)) return BadRequest("user is taken");
            
            var user =_mapper.Map<AppUser>(registerDto);
            //using var hmac = new HMACSHA512();//hash salt assign random value to password and use hashing algoirthm
            //keyword using class removed from memory -( garbage collection)  is used because if any unused class is present it will be disposed so we this dispose is done by using keyword.
            
            
                user.UserName =registerDto.username.ToLower();
               // user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password));
               // user.PasswordSalt=hmac.Key;
            
                var result = await _userManager.CreateAsync(user, registerDto.password);

                if(!result.Succeeded) return BadRequest(result.Errors);

                var roleResult = await _userManager.AddToRoleAsync(user, "Member");
                if(!roleResult.Succeeded) return BadRequest(result.Errors);
            
            return new UserDto
            {
                username =user.UserName,
                Token = await _TokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender =user.Gender
                
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user= await _userManager.Users
            .Include(p=>p.Photos)
            .SingleOrDefaultAsync(x => x.UserName ==loginDto.username);
            if (user == null) return Unauthorized("Invalid username");
            //password check
            //using var hmac = new HMACSHA512(user.PasswordSalt);//algorithm to check password salt
            //var ComputeHash =hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

            // apply for loop to check each element of a password in byte
            /*for(int i=0; i< ComputeHash.Length; i++){
                if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }*/

            var result = await _userManager.CheckPasswordAsync(user, loginDto.password);
            if(!result) return BadRequest("Invalid password");

            return new UserDto
            {
                username =user.UserName,
                Token =await _TokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x=> x.IsMain)?.Url,
                KnownAs =user.KnownAs,
                Gender =user.Gender
            };

        }
    private async Task<bool> UserExists(string username)
    {
        return await _userManager.Users.AnyAsync(x =>x.UserName==username.ToLower());
    }

    }

}