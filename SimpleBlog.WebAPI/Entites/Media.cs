namespace SimpleBlog.WebAPI.Entites
{
    public class Media
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int? PostId { get; set; }  // Optional FK to a Post
        public Post Post { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
