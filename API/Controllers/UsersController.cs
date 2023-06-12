using System;
using API.entites;
using API.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{   [ApiController]//Attribute
    [Route("api/[controller]")]//api/users
    
    public class UsersController : ControllerBase
    {   
        
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
               
        }
        [HttpGet]//endpoint
        //ActionResult produces response type 200 ok success
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
            var users = await _context.Users.ToListAsync();
            return users;
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<AppUser>> GetUser(int Id)
        {
            return await _context.Users.FindAsync(Id);
                  
        }
    }
}