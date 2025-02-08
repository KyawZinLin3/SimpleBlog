namespace SimpleBlog.WebAPI.Entites
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }      
        public string Content { get; set; }
        public bool Status { get; set; }      
        public string AuthorId { get; set; }    
        public ApplicationUser Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
        public ICollection<Media> MediaItems { get; set; } = new List<Media>();
    }
}
