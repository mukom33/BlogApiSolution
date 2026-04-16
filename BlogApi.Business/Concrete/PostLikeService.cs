using AutoMapper;
using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;
using BlogApi.DataAccess.Abstract;
using BlogApi.Business.Abstract;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration.UserSecrets;
using BlogApi.Business.Wrappers;

namespace BlogApi.Business.Concrete
{
    public class PostLikeService : IPostLikeService
    {
        private readonly IGenericRepository<PostLike> _repository;
        private readonly IGenericRepository<Post> _postRepository;
        private readonly IMapper _mapper;
        public PostLikeService(IGenericRepository<PostLike> repository, IMapper mapper, IGenericRepository<Post> postRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _postRepository = postRepository;
        }

        public async Task<ApiResponse<PostLike>> CreatePostLikeAsync(int PostId,int userId)
        {
            PostLike? postlike = null;
            try
            {
                int postCount = await _postRepository.CountAsync(p => p.PostId == PostId);
                bool existPost = postCount > 0;

                if(!existPost)
                    return ApiResponse<PostLike>.FailResponse("bu post mevcut değil");

                var count = await _repository.CountAsync(p => p.AppUserId == userId && p.PostId == PostId); 
                if(count > 0)
                    return ApiResponse<PostLike>.FailResponse("Bu postu zaten beğendiniz");

                postlike = new PostLike();

                postlike.CreatedAt = DateTime.UtcNow;
                postlike.AppUserId = userId;
                postlike.PostId = PostId;
                
                await _repository.AddAsync(postlike);

                var result = await _repository.SaveAsync();
                if(result == 0)
                    return ApiResponse<PostLike>.FailResponse("İşlem başarısız");
            }
            catch (Exception ex)
            {
                return ApiResponse<PostLike>.FailResponse(ex.Message);
            }

            return ApiResponse<PostLike>.SuccessResponse(postlike);
        }

        public async Task<ApiResponse<PostLike>> DeletePostLike(int id,int userId)
        {   
                  
            var postlike = await _repository.GetAsync(i => i.PostLikeId == id);
            if(postlike == null)
            {
                return ApiResponse<PostLike>.FailResponse("Böyle bir postLike Bulunamadı bulunamadı");
            }
            
            if(postlike.AppUserId != userId)
            {
                return ApiResponse<PostLike>.FailResponse("Delete işlemini yapmaya yetkiniz yoktur");
            }
            
            try
            {
                _repository.Remove(postlike);

                await _repository.SaveAsync();
            }
            catch (Exception ex)
            {
                return ApiResponse<PostLike>.FailResponse(ex.Message);
            }

            return ApiResponse<PostLike>.SuccessResponse(postlike,"Silme işlemi başarılı");
        }

        public async Task<ApiResponse<List<ListPostLikeDTO>>> GetPostByPostLikes(int id)
        {
            var getposts = await _repository.GetAsync(i=>i.PostId == id);
            if(getposts == null)
            {
                return ApiResponse<List<ListPostLikeDTO>>.FailResponse("Böyle bir post bulunamadı");
            }

            var getpostbypostlike = await _repository.GetListAsync(i => i.PostId == id,p=>p.AppUser,p=>p.Post);

            var getpostbypostlikedto =   getpostbypostlike.Select(p =>
                                new ListPostLikeDTO
                                    {
                                        UserName = p.AppUser?.UserName ?? string.Empty,
                                        CreatedAt = p.CreatedAt,
                                        PostLikeId = p.PostLikeId,
                                        UserId = p.AppUserId
                                    }).ToList();
            
            return ApiResponse<List<ListPostLikeDTO>>.SuccessResponse(getpostbypostlikedto,"İstenen Post'un Beğenileri");
        }   
    }
}