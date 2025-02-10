using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<GetPost>> GetAllPostsByUserId(string userId) => await _customPostRepository.GetPostsByUserId(userId);

        public async Task<GetPostDetail> GetPostById(int Id)
        {
            var post = await _customPostRepository.GetPostById(Id);
            if (post == null) throw new NullReferenceException("Post is not found");
            return post;
        }

        public async Task<Post> GetNormalPostById(int Id)
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

        public async Task<bool> UpdatePost(UpdatePost updatePost)
        {
            try
            {
                var post = await _customPostRepository.GetPostByIdAsync(updatePost.Id);
                post.Title = updatePost.Title;
                post.Content = updatePost.Content;
                post.Status = updatePost.Status;
                post.AuthorId = updatePost.AuthorId;
                post.UpdatedAt = DateTime.UtcNow;

                post.PostTags.Clear();

                if (updatePost.PostTags.Count > 0)
                {
                    foreach (var tag in updatePost.PostTags)
                    {
                        post.PostTags.Add(new PostTag { TagId = tag.TagId, PostId = post.Id });
                    }
                }
                //_postRepository.Update(post);
                var changes = await _postRepository.SaveChangesAsync();
                if (changes > 0)
                    return true;

                return false;
            }
           catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
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
