using BlogApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogApi.Domain.Entities
{
    public class AppUser:IdentityUser<int>
    {
        public string FullName { get; set; }=null!;
        public DateTime? DateAdded{get;set;}
        public List<Post>Posts{get;set;} = new List<Post>();
        public List<Comment>Comments{get;set;} = new List<Comment>();
        public List<PostLike>PostLikes{get;set;} = new List<PostLike>();
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}