using SimpleBlog.WebAPI.Entites;
using SimpleBlog.WebAPI.Models.Post;
using SimpleBlog.WebAPI.Repositories.Base;

namespace SimpleBlog.WebAPI.Repositories.PostRepo
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<List<GetPost>> GetPosts();
    }
}
