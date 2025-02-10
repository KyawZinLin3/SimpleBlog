using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.UI.Models;
using SimpleBlog.UI.Services;
using System.Diagnostics;

namespace SimpleBlog.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductService _productService;
        private readonly AuthService _authService;
        private readonly PostService _postService;

        public HomeController(ILogger<HomeController> logger,
                              ProductService productService,
                              AuthService authService,
                              PostService postService)
        {
            _logger = logger;
            _productService = productService;
            _authService = authService;
            _postService = postService;
        }


        public async Task<IActionResult> Index()
        {
            var contents = await _postService.GetPost();
            return View(contents);  
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize] 
        public async Task<IActionResult> SecureProducts()
        {
            var token = _authService.GetToken(); 
            var products = await _productService.GetSecureProductsAsync(token);
            return View(products);
        }
    }
}
