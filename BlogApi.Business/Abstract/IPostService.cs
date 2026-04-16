using BlogApi.Business.DTOs;
using BlogApi.Business.Wrappers;
using BlogApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogApi.Business.Abstract
{
    public interface IPostService
    {
       
        
         Task<ApiResponse<List<ListPostDTO>>> GetPostByIdAsync(int id);
        
        
        Task<ApiResponse<PostDTO>> UpdatePostAsync(int id, PostDTO dto,int userId);
        Task<ApiResponse<Post>> DeletePostAsync(int id,int userId);
        Task<ApiResponse<PostDTO>> CreatePostAsync(int userId, PostDTO request);
        
        
        Task<ApiResponse<PagedDTO<ListPostDTO>>> GetPagedUserPosts(int id, int page, int pageSize);        Task<ApiResponse<PagedDTO<ListPostDTO>>>GetAllPosts(int page,int pageSize);
        
        // Task<List<ListPostDTO>>GetPostsByUserId(int id);

    }
}