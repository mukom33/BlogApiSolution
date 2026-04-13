using BlogApi.Domain;

namespace BlogApi.Domain.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public AppUser AppUser {get;set;} = null!;
        public List<Tag>Tags {get;set;} = new();
        public List<Comment>Comments {get;set;} = new();
        public int AppUserId {get;set;} 
        public List<PostLike>PostLikes {get;set;} = new(); 
        public DateTime? DeletedAt { get; set; }
    }
}
