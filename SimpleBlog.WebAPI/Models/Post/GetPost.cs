using SimpleBlog.WebAPI.Models.Tag;

namespace SimpleBlog.WebAPI.Models.Post
{
    public class GetPost
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public List<GetTag> Tags { get; set; }
        public int Comments { get; set; }
        public DateTime TimeAgo { get; set; }
    }
}
