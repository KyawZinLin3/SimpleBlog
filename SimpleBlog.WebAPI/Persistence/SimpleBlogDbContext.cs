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



            builder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(200);


                entity.Property(e => e.Content)
                      .IsRequired();

                entity.Property(e => e.Status)
                      .IsRequired();

                entity.Property(e => e.CreatedAt)
                      .IsRequired();

                entity.Property(e => e.UpdatedAt)
                      .IsRequired();

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

                entity.HasData(
                        new Tag { Id = 1, Name = "C#", CreatedAt = DateTime.Now },
                        new Tag { Id = 2, Name = "JavaScript", CreatedAt = DateTime.Now },
                        new Tag { Id = 3, Name = "Python", CreatedAt = DateTime.Now },
                        new Tag { Id = 4, Name = "Java", CreatedAt = DateTime.Now },
                         new Tag { Id = 5, Name = "Asp.Net Core", CreatedAt = DateTime.Now }
                );
            });

            builder.Entity<PostTag>(entity =>
            {
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

            builder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Content)
                      .IsRequired();

                entity.Property(e => e.CreatedAt)
                      .IsRequired();

                entity.HasOne(e => e.Post)
                      .WithMany(p => p.Comments)
                      .HasForeignKey(e => e.PostId)
                      .OnDelete(DeleteBehavior.Cascade);



                entity.HasOne(e => e.User)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            builder.Entity<Media>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Url)
                      .IsRequired();

                entity.Property(e => e.UploadedAt)
                      .IsRequired();


                entity.HasOne(e => e.Post)
             .WithMany(p => p.MediaItems)
             .HasForeignKey(e => e.PostId)
             .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }

}
