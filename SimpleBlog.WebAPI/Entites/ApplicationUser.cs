using Microsoft.AspNetCore.Identity;

namespace SimpleBlog.WebAPI.Entites
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
