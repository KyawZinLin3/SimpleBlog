using Microsoft.AspNetCore.Mvc;

namespace SimpleBlog.WebAPI.Controllers
{
    public class MediaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
