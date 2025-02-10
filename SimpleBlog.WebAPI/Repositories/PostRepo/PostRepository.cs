using Microsoft.EntityFrameworkCore;
using SimpleBlog.WebAPI.Entites;
using SimpleBlog.WebAPI.Models.Post;
using SimpleBlog.WebAPI.Models.Tag;
using SimpleBlog.WebAPI.Persistence;
using SimpleBlog.WebAPI.Repositories.Base;

namespace SimpleBlog.WebAPI.Repositories.PostRepo
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(SimpleBlogDbContext context) : base(context)
        {

        }

        public async Task<List<GetPost>> GetPosts()
        {
            var posts = await _dbSet.Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new GetPost
                {
                    Id = p.Id,
                    Author = p.Author.FullName,
                    Title = p.Title,
                    Tags = p.PostTags.Select(t => new GetTag { Id = t.Tag.Id, Name = t.Tag.Name }).ToList(),
                    Comments = p.Comments.Count,
                    TimeAgo = p.CreatedAt
                })
                .ToListAsync();
            return posts;
        }

        public async Task<List<GetPost>> GetPostsByUserId(string userId)
        {
            var posts = await _dbSet.Where(p=> p.AuthorId == userId)
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new GetPost
                {
                    Id = p.Id,
                    Author = p.Author.FullName,
                    Title = p.Title,
                    Tags = p.PostTags.Select(t => new GetTag { Id = t.Tag.Id, Name = t.Tag.Name }).ToList(),
                    Comments = p.Comments.Count,
                    TimeAgo = p.CreatedAt
                })
                .ToListAsync();
            return posts;
        }

        public async Task<GetPostDetail> GetPostById(int postId)
        {
            var post = await _dbSet
                                .Where(p => p.Id == postId) 
                                .Include(p=>p.PostTags).ThenInclude(pt => pt.Tag)
                                .Select(p => new GetPostDetail
                                {
                                    Id = p.Id,
                                    Author = p.Author.FullName, 
                                    Title = p.Title,
                                    Tags = p.PostTags.Select(pt => new GetTag { Id = pt.Tag.Id, Name = pt.Tag.Name }).ToList(),
                                    CreatedAt = p.CreatedAt,
                                    Content = p.Content
                                })
                                .FirstOrDefaultAsync();
            return post;
        }

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            var post = await _dbSet
                                .Where(p => p.Id == postId)
                                .Include(p => p.PostTags)
                                .ThenInclude(pt => pt.Tag)
                                .FirstOrDefaultAsync();
            return post;
        }
    }
}
