using BlogApi.Domain.Entities;

namespace BlogApi.Business.DTOs
{
    public class TagDTO
    {
        public string TagName { get; set; } = string.Empty;
        public int PostId { get; set; } 

    }
}