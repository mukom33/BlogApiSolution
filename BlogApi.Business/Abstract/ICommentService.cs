using BlogApi.Business.DTOs;
using BlogApi.Business.Wrappers;
using BlogApi.Domain.Entities;

namespace BlogApi.Business.Abstract
{
    public interface ICommentService
    {
       // Task<CommentDTO>GetCommentByIdAsync(int id);

        Task<ApiResponse<CommentDTO>> CreateAsync(int PostId,int userId,CommentDTO request);
        Task<ApiResponse<CommentDTO>> UpdateAtAsync(int id, CommentDTO request,int userId);
        Task<ApiResponse<Comment>> DeletedAsync(int id,int userId);
        
        Task<ApiResponse<PagedDTO<ListCommentDTO>>>GetPagedCommentsByPostId(int id,int page,int pageSize);
        //Task<List<ListCommentDTO>>GetCommentsByPostId(int id);
    }
}