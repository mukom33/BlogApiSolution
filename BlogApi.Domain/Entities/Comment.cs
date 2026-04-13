using BlogApi.Domain;

namespace BlogApi.Domain.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; } = string.Empty;
        public Post Post {get;set;} = null!;
        public AppUser AppUser {get;set;} = null!;
        public int AppUserId {get;set;}
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}