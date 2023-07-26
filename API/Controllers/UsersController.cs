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
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;

namespace API.Controllers
{   
    [Authorize]
    public class UsersController : BaseApiController
    {   
        
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
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
            
            var user=await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user==null) return NotFound();
            _mapper.Map(memberUpdateDto, user);

            if(await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }
        [HttpPost("add-Photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user =await _userRepository.GetUserByUsernameAsync(User.GetUsername());
             
            if(user == null) return NotFound();
            var result =await _photoService.AddPhotoAsync(file);
            if(result.Error != null) return BadRequest(result.Error.Message);
            var photo =new Photo{
                Url =result.SecureUrl.AbsoluteUri,
                PublicId =result.PublicId
            };
            if (user.Photos.Count ==0) photo.IsMain =true;
            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync()) {
                return CreatedAtAction(nameof(GetUser),new{username =user.UserName},_mapper.Map<PhotoDto>(photo));
            }
            return BadRequest("problem adding photo");
        }
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if(user == null) return NotFound();
            var photo =user.Photos.FirstOrDefault(x=> x.id == photoId);
            if(photo == null) return NotFound();
            if(photo.IsMain) return BadRequest("this is already your main photo");
            var currentMain =user.Photos.FirstOrDefault(x =>x.IsMain);
            if(currentMain !=null) currentMain.IsMain =false;
            photo.IsMain = true;

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("problem setting the main photo");
        }
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user =await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo =user.Photos.FirstOrDefault(x=> x.id == photoId);

            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("you cannot delete main photo");

            if(photo.PublicId != null)
            {
                var result =await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);

            }
            user.Photos.Remove(photo);

            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting photo");
        }
    }
}