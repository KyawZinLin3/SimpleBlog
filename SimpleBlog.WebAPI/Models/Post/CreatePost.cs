using SimpleBlog.WebAPI.Entites;
using SimpleBlog.WebAPI.Models.Media;
using SimpleBlog.WebAPI.Models.PostTag;

namespace SimpleBlog.WebAPI.Models.Post
{
    public class CreatePost
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
        public string AuthorId { get; set; }

        // Navigation properties
        //public List<Comment> Comments { get; set; } 
        public List<CreatePostTag> PostTags { get; set; } 
        public List<CreateMedia> MediaItems { get; set; }
    }
}
