using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;

namespace BlogApi.Business.Abstract
{
    public interface IPostLikeService
    {
        
        Task<PostLikeDTO>GetPostLikeById(int id);


        Task<bool>DeletePostLike(int id,int userId);
        Task<PostLike>CreateTagAsync(PostLikeDTO request);


        Task<PagedDTO<PostLikeDTO>>GetPostPostLikes(int page,int pageSize,int id);
        Task<PagedDTO<PostLikeDTO>>GetUserPostlikes(int page,int pageSize,int id);


        Task<List<PostLikeDTO>>GetPostLikeByPostId(int id);
        Task<List<PostLikeDTO>>GetPostLikeByUserId(int id);
    }
}