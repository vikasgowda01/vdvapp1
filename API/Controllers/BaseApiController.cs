using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
        [ApiController]//Attribute
        [Route("api/[controller]")]//api/users
        public class BaseApiController : ControllerBase
        {

        }
    
}