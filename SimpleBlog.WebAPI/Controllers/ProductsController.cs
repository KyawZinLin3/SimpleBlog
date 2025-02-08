using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleBlog.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = new List<object>
            {
                new {Id = 1, Name = "Laptop",Price = 1200},
                new {Id = 2, Name = "Phone",Price = 800}
            };
            return Ok(products);
        }

        [HttpGet("secure")]
        [Authorize]
        public IActionResult GetSecureProducts()
        {
            var products = new List<string> { "Secure Product 1", "Secure Product 2" };
            return Ok(products);
        }
    }
}
