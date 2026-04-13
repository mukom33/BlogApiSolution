using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;

namespace BlogApi.Business.Abstract
{
    public interface ICommentService
    {
        Task<CommentDTO>GetCommentByIdAsync(int id);
       
        Task<bool>CreateAsync(int userId,CommentDTO request);
        Task<bool>UpdateAtAsync(int id,CommentDTO request,int userId);
        Task<bool>DeletedAsync(int id,int userId);
        Task<PagedDTO<CommentDTO>>GetPagedCommentsByUserId(int id,int page,int pageSize);
        Task<PagedDTO<CommentDTO>>GetPagedCommentsByPostId(int id,int page,int pageSize);
        Task<List<CommentDTO>>GetCommentsByPostId(int id);
        Task<List<CommentDTO>>GetCommentsByUserId(int id);
    }
}