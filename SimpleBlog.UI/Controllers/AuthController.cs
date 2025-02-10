using Microsoft.AspNetCore.Mvc;
using SimpleBlog.UI.Models.Auth;
using SimpleBlog.UI.Services;

namespace SimpleBlog.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User userModel)
        {
            if (ModelState.IsValid)
            {
                var success = await _authService.RegisterAsync(userModel);
                if(!success)
                {
                    ViewBag.Error = "Registration failed.";
                    return View();
                }
                ViewBag.Message = "Registration Successful!";
                return RedirectToAction("Success");
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Success()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login loginModel)
        {

            var success = await _authService.LoginAsync(loginModel.Email, loginModel.Password);
            if (success)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        public IActionResult Logout()
        {
            _authService.Logout();
            return RedirectToAction("Login");
        }

       

    }
}
