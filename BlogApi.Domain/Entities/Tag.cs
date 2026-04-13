namespace BlogApi.Domain.Entities
{
    public class Tag
    {
        public int TagId { get; set; }  
        public string TagName { get; set; } = string.Empty;
        public List<Post>Posts{get;set;} = new List<Post>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}