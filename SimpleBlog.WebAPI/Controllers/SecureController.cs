using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleBlog.WebAPI.Controllers
{
    [Route("api/secure")]
    [ApiController]
    [Authorize]
    public class SecureController : ControllerBase
    {
        [HttpGet]
        
        public IActionResult GetSecureData()
        {
            return Ok(new { message = "This is a protected API route." });
        }
    }
}
