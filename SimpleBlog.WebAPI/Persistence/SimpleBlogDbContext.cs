using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleBlog.WebAPI.Entites;
using System.Reflection.Emit;

namespace SimpleBlog.WebAPI.Persistence
{
    public class SimpleBlogDbContext : IdentityDbContext<ApplicationUser>
    {
        public SimpleBlogDbContext(DbContextOptions<SimpleBlogDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Media> MediaItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Additional customizations (if needed)

            // --- Post Configuration ---
            builder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                //entity.Property(e => e.Slug)
                //      .IsRequired()
                //      .HasMaxLength(200);

                entity.Property(e => e.Content)
                      .IsRequired();

                entity.Property(e => e.Status)
                      .IsRequired();

                entity.Property(e => e.CreatedAt)
                      .IsRequired();

                entity.Property(e => e.UpdatedAt)
                      .IsRequired();

                // Each Post has one Author (ApplicationUser)
                entity.HasOne(e => e.Author)
                      .WithMany(u => u.Posts)
                      .HasForeignKey(e => e.AuthorId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- Tag Configuration ---
            builder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                      .IsRequired();
            });

            // --- PostTag Configuration (Join Table) ---
            builder.Entity<PostTag>(entity =>
            {
                // Composite primary key
                entity.HasKey(pt => new { pt.PostId, pt.TagId });

                entity.HasOne(pt => pt.Post)
                      .WithMany(p => p.PostTags)
                      .HasForeignKey(pt => pt.PostId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pt => pt.Tag)
                      .WithMany(t => t.PostTags)
                      .HasForeignKey(pt => pt.TagId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- Comment Configuration ---
            builder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Content)
                      .IsRequired();

                entity.Property(e => e.CreatedAt)
                      .IsRequired();

                // Each Comment is linked to one Post
                entity.HasOne(e => e.Post)
                      .WithMany(p => p.Comments)
                      .HasForeignKey(e => e.PostId)
                      .OnDelete(DeleteBehavior.Cascade);


                // Each Comment is linked to one ApplicationUser.
                // Cascade delete is disabled (Restrict) to avoid multiple cascade paths.
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // --- Media Configuration ---
            builder.Entity<Media>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Url)
                      .IsRequired();

                entity.Property(e => e.UploadedAt)
                      .IsRequired();

                // Media may be optionally associated with a Post.
                entity.HasOne(e => e.Post)
             .WithMany(p => p.MediaItems)
             .HasForeignKey(e => e.PostId)
             .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
