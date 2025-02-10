using Microsoft.AspNetCore.Mvc;
using SimpleBlog.UI.Models.Content;
using SimpleBlog.UI.Services;

namespace SimpleBlog.UI.Controllers
{
    public class ContentController : Controller
    {
        private readonly TagService _tagService;
        private readonly PostService _postService;
        public ContentController(TagService tagService, PostService postService)
        {
            _tagService = tagService;
            _postService = postService;
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tags = await _tagService.GetTagsAsync();

            var viewModel = new CompositeContent
            {
                Tags = tags,
                FormData = new CreateContent()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CompositeContent model, string actionType)
        {
            if (ModelState.IsValid)
            {
                var status = actionType == "Publish" ? true : false;

                await _postService.CreatePost(model.FormData, status);

                return RedirectToAction("Index", "Home");
            }

            
            var tags = await _tagService.GetTagsAsync();

            model.Tags = tags;
            //ViewBag.Message = ModelState.

            return View(model);
        }
    }
}
