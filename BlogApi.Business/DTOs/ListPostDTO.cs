namespace BlogApi.Business.DTOs
{
    public class ListPostDTO
    {
        public string Title { get; set; } = null!;
        public int TotalComment { get; set; } 
        public int TotalLike { get; set; }
        public string  UserName { get; set; } = null!;
        public int UserId{get;set;}
        public DateTime CreatedAt { get; set; }
        public int PostId { get; set; }
    }
}
