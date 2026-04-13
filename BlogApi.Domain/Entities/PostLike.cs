using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace BlogApi.Domain.Entities
{
    public class PostLike
    {

        public int PostLikeId {get;set;}
        
        public AppUser AppUser{get;set;} = null!;
        public Post Post{get;set;}= null!;
        public int AppUserId { get; set; }
        public int PostId {get;set;}
        public DateTime CreatedAt { get; set; }
        public DateTime? Delete {get;set;}
    }
}