using Microsoft.AspNetCore.Mvc;
using SimpleBlog.UI.Services;

namespace SimpleBlog.UI.Controllers
{
    public class ProfileController : Controller
    {
        private readonly PostService _postService;
        private readonly AuthService _authService;
        public ProfileController(PostService postService,
                                 AuthService authService)
        {
            _postService = postService;
            _authService = authService;
        }
        public async Task<IActionResult> ProfileView()
        {

            var token = _authService.GetToken();
            var posts = await _postService.GetPostByUser(token);
            return View(posts);
        }
    }
}
