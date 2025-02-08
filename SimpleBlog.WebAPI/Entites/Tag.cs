namespace SimpleBlog.WebAPI.Entites
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public DateTime CreatedAt { get; set; }
        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
