using SimpleBlog.WebAPI.Entites;
using SimpleBlog.WebAPI.Models.Post;
using SimpleBlog.WebAPI.Repositories.Base;

namespace SimpleBlog.WebAPI.Repositories.PostRepo
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<GetPostDetail> GetPostById(int postId);
        Task<Post> GetPostByIdAsync(int postId);
        Task<List<GetPost>> GetPosts();
        Task<List<GetPost>> GetPostsByUserId(string userId);
    }
}
