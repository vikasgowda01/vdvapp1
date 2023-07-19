using System;
using API.entites;
using API.data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using API.Extensions;
using API.DTOs;
using AutoMapper;
using System.Security.Claims;

namespace API.Controllers
{   
    [Authorize]
    public class UsersController : BaseApiController
    {   
        
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
               
        }
        //[AllowAnonymous]
        [HttpGet]//endpoint
        //ActionResult produces response type 200 ok success
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){
            var users = await _userRepository.GetMembersAsync();

            //var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(users);
            
        }
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
              
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user=await _userRepository.GetUserByUsernameAsync(username);

            if(user==null) return NotFound();
            _mapper.Map(memberUpdateDto, user);

            if(await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }
    }
}