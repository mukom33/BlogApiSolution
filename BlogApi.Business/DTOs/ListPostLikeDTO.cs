namespace BlogApi.Business.DTOs
{
    public class ListPostLikeDTO
    {
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public  int PostLikeId { get; set; }
    }
}
