using AutoMapper;
using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;
using BlogApi.DataAccess.Abstract;
using BlogApi.Business.Abstract;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace BlogApi.Business.Concrete
{
    public class PostLikeService : IPostLikeService
    {
        private readonly IGenericRepository<PostLike> _repository;
        private readonly IMapper _mapper;
        public PostLikeService(IGenericRepository<PostLike> repository,IMapper mapper)      
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PostLike> CreateTagAsync(PostLikeDTO dto)
        {
            var postlike = _mapper.Map<PostLike>(dto);
            postlike.CreatedAt = DateTime.UtcNow;

            await _repository.AddAsync(postlike);
            await _repository.SaveAsync();

            return postlike;
        }

        public async Task<bool> DeletePostLike(int id,int userId)
        {   
                  
            var postlike = await _repository.GetAsync(i => i.PostLikeId == id);
            if(postlike.AppUserId != userId)
            {
                throw new Exception("Beğeniyi silmeye yetkiniz yok");
            }

            _repository.Remove(postlike);
            await _repository.SaveAsync();

            return true;
        }

        public async Task<PostLikeDTO> GetPostLikeById(int id)
        {
            var postlike = await  _repository.GetAsync(i => i.PostLikeId == id);
            if(postlike == null)
            {
                return null;
            }

            return _mapper.Map<PostLikeDTO>(postlike);
        }

        public async Task<List<PostLikeDTO>> GetPostLikeByPostId(int id)
        {
            var getpostlikebypostıd = await _repository.GetListAsync(i => i.PostId == id);
            if(getpostlikebypostıd == null)
            {
                return null;
            }
            
            var getpostlikebypostıdto =_mapper.Map<List<PostLikeDTO>>(getpostlikebypostıd);
            
            return getpostlikebypostıdto;
        }

        public async Task<List<PostLikeDTO>> GetPostLikeByUserId(int id)
        {
            var getpostlikebyuserıd = await _repository.GetListAsync(i => i.AppUserId == id);
            if(getpostlikebyuserıd == null)
            {
                return null;
            }

            var getpostlikebyuserdto = _mapper.Map<List<PostLikeDTO>>(getpostlikebyuserıd);
            
            return getpostlikebyuserdto;
        }

        public async Task<PagedDTO<PostLikeDTO>> GetPostPostLikes(int page,int pageSize,int id)
        {
            var getpostbypostlike = await _repository.GetPagedAsync(page,pageSize,i => i.PostId == id);
            return new PagedDTO<PostLikeDTO>
            {
                Items = _mapper.Map<List<PostLikeDTO>>(getpostbypostlike.Items),
                TotalCount = getpostbypostlike.TotalCount,
                Page = getpostbypostlike.Page,
                PageSize = getpostbypostlike.PageSize
            };
        }

        public async Task<PagedDTO<PostLikeDTO>> GetUserPostlikes(int page,int pageSize,int id)
        {
            var getuserbypostlikes = await _repository.GetPagedAsync(page,pageSize,i => i.AppUserId == id);
            return new PagedDTO<PostLikeDTO>
            {
                Items = _mapper.Map<List<PostLikeDTO>>(getuserbypostlikes.Items),
                TotalCount = getuserbypostlikes.TotalCount,
                Page = getuserbypostlikes.Page,
                PageSize = getuserbypostlikes.PageSize
            };
        }        
    }
}