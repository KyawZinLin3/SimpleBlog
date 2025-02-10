using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.UI.Models.Content
{
    public class CreateContent
    {
        //[Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        //[Required(ErrorMessage = "At least one tag is required")]
        public List<int> TagId { get; set; } = new List<int>();

        //[Required(ErrorMessage = "Content cannot be empty")]
        public string Content { get; set; }

        //public IFormFile CoverImage { get; set; }
    }
}
