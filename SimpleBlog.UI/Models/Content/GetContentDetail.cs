using SimpleBlog.UI.Models.Tag;

namespace SimpleBlog.UI.Models.Content
{
    public class GetContentDetail
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public List<GetTag> Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
    }
}
