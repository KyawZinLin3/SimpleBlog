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

        public HomeController(ILogger<HomeController> logger,
                              ProductService productService,
                              AuthService authService)
        {
            _logger = logger;
            _productService = productService;
            _authService = authService;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductsAsync();
            return View(products);  
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
            var token = _authService.GetToken(); // Retrieve token from cookie
            var products = await _productService.GetSecureProductsAsync(token);
            return View(products);
        }
    }
}
