using BlogApi.Business.DTOs;
using BlogApi.Business.Wrappers;
using BlogApi.Domain.Entities;

namespace BlogApi.Business.Abstract
{
    public interface IPostLikeService
    {
        
      //  Task<PostLikeDTO>GetPostLikeById(int id);


         Task<ApiResponse<PostLike>> DeletePostLike(int id,int userId);
        Task<ApiResponse<PostLike>> CreatePostLikeAsync(int PostId,int userId);


        Task<ApiResponse<List<ListPostLikeDTO>>> GetPostByPostLikes(int id);
       // Task<PagedDTO<PostLikeDTO>>GetUserPostlikes(int page,int pageSize,int id);


      //  Task<List<PostLikeDTO>>GetPostLikeByPostId(int id);
      //  Task<List<PostLikeDTO>>GetPostLikeByUserId(int id);
    }
}