using SimpleBlog.UI.Models.Tag;

namespace SimpleBlog.UI.Models.Content
{
    public class CompositeContent
    {
        public List<GetTag>? Tags { get; set; }

        public CreateContent FormData { get; set; }
    }
}
