namespace BlogApi.Business.DTOs
{
    public class ListCommentDTO
    {
        public string UserName { get; set; } = null!;
        public string Text {get;set;} = null!;
        public DateTime CreatedAt {get;set;}
        public int UserId { get; set; }
        public int CommentId { get; set; }
    }
}