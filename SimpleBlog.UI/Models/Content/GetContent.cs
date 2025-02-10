using SimpleBlog.UI.Models.Tag;

namespace SimpleBlog.UI.Models.Content
{
    public class GetContent
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public List<GetTag> Tags { get; set; }
        public int Comments { get; set; }
        public DateTime TimeAgo { get; set; }
    }
}
