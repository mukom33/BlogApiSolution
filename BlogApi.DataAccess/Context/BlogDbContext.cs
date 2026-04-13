using BlogApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.DataAccess.Context 
{
    public class BlogApiContext :IdentityDbContext<AppUser,AppRole,int>
    {
         public BlogApiContext(DbContextOptions<BlogApiContext> options):base(options)
        {
            
        }
        public DbSet<Post>Posts{get;set;}
        public DbSet<PostLike>PostLikes{get;set;}
        public DbSet<Tag>Tags{get;set;}
        public DbSet<Comment>Comments{get;set;}
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Post>().HasOne(u=>u.AppUser).WithMany(p=>p.Posts).HasForeignKey(u => u.AppUserId);
            modelBuilder.Entity<Comment>().HasOne(c =>c.Post).WithMany(p =>p.Comments).HasForeignKey(c => c.PostId);
            modelBuilder.Entity<Comment>().HasOne(c => c.AppUser).WithMany(a => a.Comments).HasForeignKey(c => c.AppUserId);
            modelBuilder.Entity<PostLike>().HasOne(pl => pl.AppUser).WithMany(a => a.PostLikes).HasForeignKey(pl => pl.AppUserId);
            modelBuilder.Entity<PostLike>().HasOne(pl => pl.Post).WithMany(p => p.PostLikes).HasForeignKey(pl => pl.PostId);
           

            modelBuilder.Entity<Post>()
            .HasMany(e => e.Tags)
            .WithMany(e => e.Posts)
            .UsingEntity("PostTag",
                
                r => r.HasOne(typeof(Tag)).WithMany().HasForeignKey("TagId").HasPrincipalKey(nameof(Tag.TagId)),
                l => l.HasOne(typeof(Post)).WithMany().HasForeignKey("PostId").HasPrincipalKey(nameof(Post.PostId)),
                j => j.HasKey("PostId", "TagId"));
            
            modelBuilder.Entity<PostLike>()
                .HasIndex(pl => new { pl.AppUserId, pl.PostId })
                .IsUnique();

            var hasher = new PasswordHasher<AppUser>();

            var user2 = new AppUser
            {
                Id = 2,
                UserName = "muhammed",
                NormalizedUserName = "MUHAMMED",
                Email = "muhammed@test.com",
                NormalizedEmail = "MUHAMMED@TEST.COM",
                FullName = "Muhammed Yetimçok",
                DateAdded = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                DeletedAt = null,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            user2.PasswordHash = hasher.HashPassword(user2, "Test123*");

            var user3 = new AppUser
            {
                Id = 3,
                UserName = "ahmet",
                NormalizedUserName = "AHMET",
                Email = "ahmet@test.com",
                NormalizedEmail = "AHMET@TEST.COM",
                FullName = "Ahmet Yılmaz",
                DateAdded = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                DeletedAt = null,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            user2.PasswordHash = hasher.HashPassword(user3, "Test123*");

            modelBuilder.Entity<AppUser>().HasData(user2, user3);

            modelBuilder.Entity<Tag>().HasData(
                new Tag
                {
                    TagId = 1,
                    TagName = "CSharp",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    DeletedAt = null
                },
                new Tag
                {
                    TagId = 2,
                    TagName = ".NET",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    DeletedAt = null
                },
                new Tag
                {
                    TagId = 3,
                    TagName = "ASP.NET Core",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    DeletedAt = null
                },
                new Tag
                {
                    TagId = 4,
                    TagName = "WebAPI",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    DeletedAt = null
                }
            );
                modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    PostId = 1,
                    Title = "Introduction to C#",
                    Content = "This post explains the basics of C# programming language.",
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow,
                    AppUserId = 2,
                    DeletedAt = null
                },
                new Post
                {
                    PostId = 2,
                    Title = "Getting Started with .NET",
                    Content = "This post introduces the .NET ecosystem and its core features.",
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow,
                    AppUserId = 2,
                    DeletedAt = null
                },
                new Post
                {
                    PostId = 3,
                    Title = "Building APIs with ASP.NET Core",
                    Content = "This post explains how to build RESTful APIs using ASP.NET Core.",
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow,
                    AppUserId = 3,
                    DeletedAt = null
                }
            );

            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    CommentId = 1,
                    Text = "Very helpful article!",
                    PostId = 1,
                    AppUserId = 3,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    DeletedAt = null
                },
                new Comment
                {
                    CommentId = 2,
                    Text = "I would like to see more LINQ examples.",
                    PostId = 2,
                    AppUserId = 3,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    DeletedAt = null
                },
                new Comment
                {
                    CommentId = 3,
                    Text = "Nice explanation about Web API.",
                    PostId = 3,
                    AppUserId = 2,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    DeletedAt = null
                }
            );

            modelBuilder.Entity<PostLike>().HasData(
                new PostLike
                {
                    PostLikeId = 1,
                    PostId = 1,
                    AppUserId = 3,
                    CreatedAt = DateTime.UtcNow
                },
                new PostLike
                {
                    PostLikeId = 2,
                    PostId = 2,
                    AppUserId = 3,
                    CreatedAt = DateTime.UtcNow
                },
                new PostLike
                {
                    PostLikeId = 3,
                    PostId = 3,
                    AppUserId = 2,
                    CreatedAt = DateTime.UtcNow
                }
            );

            modelBuilder.Entity("PostTag").HasData(
                new { PostId = 1, TagId = 1 },
                new { PostId = 1, TagId = 2 },
                new { PostId = 2, TagId = 2 },
                new { PostId = 2, TagId = 3 },
                new { PostId = 3, TagId = 3 },
                new { PostId = 3, TagId = 4 }
            );

       }
    }
}