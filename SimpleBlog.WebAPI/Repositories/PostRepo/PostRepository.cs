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

        //public async Task<>
    }
}
