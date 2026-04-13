namespace BlogApi.Business.DTOs
{
    public class CommentDTO
    {
        public string Text { get; set; } = string.Empty;
        public int PostId { get; set; }

    }
}