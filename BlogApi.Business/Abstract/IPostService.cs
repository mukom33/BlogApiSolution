using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogApi.Business.Abstract
{
    public interface IPostService
    {
       
        
        Task<PostDTO>GetPostByIdAsync(int id);
        
        
        Task<bool>UpdatePostAsync(int id,PostDTO dto,int userId);
        Task<bool>DeletePostAsync(int id,int userId);
        Task<bool>CreatePostAsync(int userId,PostDTO dto);
        
        
        
        Task<PagedDTO<PostDTO>>GetPagedUserPosts(int id,int page,int pageSize);
        Task<PagedDTO<PostDTO>>GetPagedTagPosts(int id,int page,int pageSize);
        Task<PagedDTO<ListPostDTO>>GetAllPosts(int page,int pageSize);
        
        Task<List<PostDTO>>GetPostsByTagId(int id);
        Task<List<PostDTO>>GetPostsByUserId(int id);

    }
}