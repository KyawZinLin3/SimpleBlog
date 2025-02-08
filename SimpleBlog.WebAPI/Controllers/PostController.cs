using Microsoft.AspNetCore.Mvc;

namespace SimpleBlog.WebAPI.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
