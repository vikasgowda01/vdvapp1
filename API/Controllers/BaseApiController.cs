using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{        
        [ServiceFilter(typeof(LogUserActivity))]    
        [ApiController]//Attribute
        [Route("api/[controller]")]//api/users
        public class BaseApiController : ControllerBase
        {

        }
    
}