using Microsoft.AspNetCore.Mvc;

namespace SimpleBlog.WebAPI.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
