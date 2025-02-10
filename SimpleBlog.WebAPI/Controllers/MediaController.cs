using Microsoft.AspNetCore.Mvc;
using SimpleBlog.WebAPI.Entites;
using SimpleBlog.WebAPI.Models;
using SimpleBlog.WebAPI.Models.Media;
using SimpleBlog.WebAPI.Services;

namespace SimpleBlog.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MediaController : Controller
    {
        private readonly MediaService _mediaService;
        public MediaController(MediaService mediaService)
        {
            _mediaService = mediaService;
        }
        [HttpGet]
        public async Task<IActionResult> GetMedias()
        {
            var medias = await _mediaService.GetAllMedias();
            if (medias == null)
                return NotFound(ApiResponse<string>.ErrorResponse("No Medias found"));
            return Ok(ApiResponse<IEnumerable<Media>>.SuccessResponse(medias));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMediaById(int id)
        {
            var media = await _mediaService.GetMediaById(id);
            return Ok(ApiResponse<Media>.SuccessResponse(media));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedia([FromBody] CreateMedia createMedia)
        {
            var media = new Media
            {
                Url = createMedia.Url,
                PostId = createMedia.PostId,
                UploadedAt = DateTime.UtcNow
            };
            var result = await _mediaService.CreateMedia(media);
            if (!result)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Media Create Fail"));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Media created successfully"));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMedia([FromBody] UpdateMedia updateMedia)
        {
            var media = new Media
            {
                Id = updateMedia.Id,
                Url = updateMedia.Url,
                PostId = updateMedia.PostId,
            };
            var result = await _mediaService.UpdateMedia(media);
            if (!result)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Media Update Fail"));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Media updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag([FromQuery] int Id)
        {
            var result = await _mediaService.DeleteMedia(Id);

            if (!result)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Media Delete Fail"));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Media deleted successfully"));
        }
    }
}
