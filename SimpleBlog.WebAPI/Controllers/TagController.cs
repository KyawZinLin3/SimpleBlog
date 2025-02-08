using Microsoft.AspNetCore.Mvc;
using SimpleBlog.WebAPI.Entites;
using SimpleBlog.WebAPI.Models;
using SimpleBlog.WebAPI.Models.Tag;
using SimpleBlog.WebAPI.Services;

namespace SimpleBlog.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : Controller
    {
        private readonly TagService _tagService;
        public TagController(TagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTags ()
        {
             var tags = await _tagService.GetAllTags();
            if(tags == null)
                return NotFound(ApiResponse<string>.ErrorResponse("No tags found"));
            return Ok(ApiResponse<IEnumerable<Tag>>.SuccessResponse(tags));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] CreateTag createTag)
        {
            var tag = new Tag
            {
                Name = createTag.Name,
                CreatedAt = DateTime.UtcNow
            };
            var result = await _tagService.CreateTag(tag);
            if (!result)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Tag Create Fail"));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Tag created successfully"));
        }
    }
}
