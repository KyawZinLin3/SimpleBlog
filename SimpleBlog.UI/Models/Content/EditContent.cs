using System.ComponentModel.DataAnnotations;
using SimpleBlog.UI.Models.Tag;

namespace SimpleBlog.UI.Models.Content
{
    public class EditContent
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "At least one tag is required")]
        public List<int> TagId { get; set; } = new List<int>();

        [Required(ErrorMessage = "Content cannot be empty")]
        public string Content { get; set; }

        public List<GetTag> AvailableTags { get; set; } = new List<GetTag>();
    }
}
