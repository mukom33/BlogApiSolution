using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;

namespace BlogApi.Business.Abstract
{
    public interface ITagService
    {
        
        Task<TagDTO>GetTagByIdAsync(int id);
        Task<bool>CreateTagAsync(TagDTO request);
        Task<bool>UpdateTagAsync(int id,TagDTO request);
        Task<bool>DeleteTagAsync(int id);
        Task<PagedDTO<TagDTO>>GetPagedTagsByPostId(int id,int page,int pageSize);
        Task<List<TagDTO>>GetTagsByPostId(int id);
    }
}