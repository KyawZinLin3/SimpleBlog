using Microsoft.AspNetCore.Mvc;
using SimpleBlog.WebAPI.Entites;
using SimpleBlog.WebAPI.Models.Tag;
using SimpleBlog.WebAPI.Models;
using SimpleBlog.WebAPI.Services;
using SimpleBlog.WebAPI.Models.Post;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SimpleBlog.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly PostService _postService;
        private readonly ILogger<PostController> _logger;
        public PostController(PostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postService.GetAllPosts();
            if (posts == null)
                return NotFound(ApiResponse<string>.ErrorResponse("No post found"));
            return Ok(ApiResponse<IEnumerable<GetPost>>.SuccessResponse(posts));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _postService.GetPostById(id);
            return Ok(ApiResponse<GetPostDetail>.SuccessResponse(post));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePost createPost)
        {
            var post = new Post
            {
                Title = createPost.Title,
                Content = createPost.Content,
                Status = createPost.Status,
                AuthorId = createPost.AuthorId,
                CreatedAt = DateTime.UtcNow,
                PostTags = createPost.PostTags.Count > 0 ? createPost.PostTags.Select(x => new PostTag { TagId = x.TagId }).ToList() : null
            };
            var result = await _postService.CreatePost(post);
            if (!result)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Post Create Fail"));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Post created successfully"));
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePost updatePost)
        {
           
            var result = await _postService.UpdatePost(updatePost);
            if (!result)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Post Update Fail"));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Post updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost( int Id)
        {
            var post = await _postService.GetNormalPostById(Id);
            if (post.AuthorId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                var result = await _postService.DeletePost(Id);
                if (!result)
                {
                    return BadRequest(ApiResponse<string>.ErrorResponse("Post Delete Fail"));
                }
            }

            return Ok(ApiResponse<string>.SuccessResponse("Post deleted successfully"));
        }

        [HttpGet("GetByUserId")]
        [Authorize]
        public async Task<IActionResult> GetPostsByAuthorId()
        {
            foreach (var claim in User.Claims)
            {
                _logger.LogInformation($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var post = await _postService.GetAllPostsByUserId(userId);
            return Ok(ApiResponse<IEnumerable<GetPost>>.SuccessResponse(post));
        }
    }
}
