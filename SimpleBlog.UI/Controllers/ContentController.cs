using Microsoft.AspNetCore.Mvc;
using SimpleBlog.UI.Models.Content;
using SimpleBlog.UI.Services;

namespace SimpleBlog.UI.Controllers
{
    public class ContentController : Controller
    {
        private readonly TagService _tagService;
        private readonly PostService _postService;
        private readonly AuthService _authService;
        public ContentController(TagService tagService, PostService postService, AuthService authService)
        {
            _tagService = tagService;
            _postService = postService;
            _authService = authService;
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
                var status = actionType == "publish" ? true : false;

                await _postService.CreatePost(model.FormData, status);

                return RedirectToAction("Index", "Home");
            }


            var tags = await _tagService.GetTagsAsync();

            model.Tags = tags;
            //ViewBag.Message = ModelState.

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult>  Detail(int id)
        {
            var post =await  _postService.GetPostByDetail(id);
            return View(post);

        }

        [HttpPost]
        public async Task<IActionResult> Delete (int id)
        {
            var token = _authService.GetToken();
            var result = await _postService.DeletePost(id,token);
            if(!result)
            {
                return BadRequest();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            
            var post = await _postService.GetPostByDetail(id);
            var model = new EditContent
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                TagId = post.Tags.Select(t => t.Id).ToList(),
                AvailableTags = await _tagService.GetTagsAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditContent model)
        {
            if (ModelState.IsValid)
            {
                var result = await _postService.EditPost(model);
                if (result)
                {
                    return RedirectToAction("Index", "Home");
                }

            }

            model.AvailableTags =await _tagService.GetTagsAsync();
            return View(model);
        }
    }
}
