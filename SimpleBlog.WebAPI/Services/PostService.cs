using SimpleBlog.WebAPI.Entites;
using SimpleBlog.WebAPI.Models.Post;
using SimpleBlog.WebAPI.Repositories.Base;
using SimpleBlog.WebAPI.Repositories.PostRepo;
using System.Security.Claims;

namespace SimpleBlog.WebAPI.Services
{
    public class PostService
    {
        private IPostRepository _customPostRepository;
        private IRepository<Post> _postRepository;
       
        public PostService(IRepository<Post> postRepository, IHttpContextAccessor httpContextAccessor,IPostRepository customPostRepository)
        {
            _postRepository = postRepository;
            _customPostRepository = customPostRepository;
        }

      

        public async Task<IEnumerable<GetPost>> GetAllPosts() => await _customPostRepository.GetPosts();

        public async Task<Post> GetPostById(int Id)
        {
            var post = await _postRepository.GetByIdAsync(Id);
            if (post == null) throw new NullReferenceException("Post is not found");
            return post;
        }

        public async Task<bool> CreatePost(Post post)
        {
            await _postRepository.AddAsync(post);
            var changes = await _postRepository.SaveChangesAsync();
            if (changes > 0)
                return true;

            return false;
        }

        public async Task<bool> UpdatePost(Post post)
        {
            _postRepository.Update(post);
            var changes = await _postRepository.SaveChangesAsync();
            if (changes > 0)
                return true;

            return false;
        }

        public async Task<bool> DeletePost(int Id)
        {
            var post = await _postRepository.GetByIdAsync(Id);
            if (post == null) throw new NullReferenceException("Post is not found");

            _postRepository.Delete(post);
            var changes = await _postRepository.SaveChangesAsync();
            if (changes > 0)
                return true;

            return false;
        }
    }
}
