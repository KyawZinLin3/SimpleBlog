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
        public async Task<IActionResult> GetTags()
        {
            var tags = await _tagService.GetAllTags();
            if (tags == null)
                return NotFound(ApiResponse<string>.ErrorResponse("No tags found"));

            var getTags = tags.Select(
                tag => new GetTag
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    CreatedAt = tag.CreatedAt
                }
            ).ToList();
            return Ok(ApiResponse<List<GetTag>>.SuccessResponse(getTags));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById(int id)
        {
            var tag = await _tagService.GetTagById(id);
            return Ok(ApiResponse<Tag>.SuccessResponse(tag));
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

        [HttpPut]
        public async Task<IActionResult> UpdateTag([FromBody] UpdateTag updateTag)
        {
            var tag = new Tag
            {
                Id = updateTag.Id,
                Name = updateTag.Name,
            };
            var result = await _tagService.UpdateTag(tag);
            if (!result)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Tag Update Fail"));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Tag updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag([FromQuery] int Id)
        {
            var result = await _tagService.DeleteTag(Id);

            if (!result)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Tag Delete Fail"));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Tag deleted successfully"));
        }
    }
}
