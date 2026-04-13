using AutoMapper;
using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;
using BlogApi.DataAccess.Abstract;
using BlogApi.Business.Abstract;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BlogApi.Business.Concrete
{
    public class CommentService : ICommentService
    {
        private readonly IGenericRepository<Comment> _repository;
        private readonly IMapper _mapper;
        public CommentService(IGenericRepository<Comment> repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CommentDTO> GetCommentByIdAsync(int id)
        {
            var comment = await _repository.GetAsync(i => i.CommentId == id);
            if(comment == null)
            {
                return null;
            }    

            return _mapper.Map<CommentDTO>(comment);    
        }

        public async Task<bool> CreateAsync(int userId,CommentDTO request)
        {
            var commentcount = await _repository.CountAsync(c => 
                c.PostId == request.PostId&&
                c.AppUserId == userId);
            if(commentcount >= 2)
            {
                return false;
            }
            
            var comment = _mapper.Map<Comment>(request);
            comment.CreatedAt = DateTime.UtcNow;
            comment.AppUserId = userId;

            await _repository.AddAsync(comment);
            await _repository.SaveAsync();

            return true;
        }

        public async Task<bool> DeletedAsync(int id,int userId)
        {
            var comment = await _repository.GetAsync(i => i.CommentId == id);
            if(comment == null)
            {
                return false;
            }

            if(comment.AppUserId != userId)
            {
                throw new Exception("Delete yapmaya yetkiniz yoktur");
            }
            
            _repository.Remove(comment);
            try
            {
                await _repository.SaveAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<PagedDTO<CommentDTO>> GetPagedCommentsByPostId(int id, int page, int pageSize)
        {
            var comment = await _repository.GetPagedAsync(page,pageSize,i => i.PostId == id);

            return new PagedDTO<CommentDTO>
            {
                Items = _mapper.Map<List<CommentDTO>>(comment.Items),
                TotalCount = comment.TotalCount,
                Page = comment.Page,
                PageSize = comment.PageSize
            };
        }

        public async Task<List<CommentDTO>> GetCommentsByPostId(int id)
        {
            var postbycomments = await _repository.GetListAsync(i => i.PostId == id);
            if(postbycomments == null)
            {
                return new List<CommentDTO>();
            }

            var commentsDto = _mapper.Map<List<CommentDTO>>(postbycomments);
            
            return commentsDto;
        }

        public async Task<bool> UpdateAtAsync(int id, CommentDTO request,int userId)
        {
            var comment = await _repository.GetAsync(i => i.CommentId == id);
            if(comment == null)
            {
                return false;
            }
            if(comment.AppUserId != userId)
            {
                throw new Exception("UserId yanlış");
            }

            _mapper.Map(request,comment);
            comment.UpdatedAt = DateTime.UtcNow;
            try
            {
                await _repository.SaveAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<PagedDTO<CommentDTO>> GetPagedCommentsByUserId(int id, int page, int pageSize)
        {
            var comment = await _repository.GetPagedAsync(page,pageSize,i => i.AppUserId == id);
            
            return new PagedDTO<CommentDTO>
            {
                Items = _mapper.Map<IEnumerable<CommentDTO>>(comment.Items),
                Page = comment.Page,
                PageSize = comment.PageSize,
                TotalCount = comment.TotalCount
            };
        }

        public async Task<List<CommentDTO>> GetCommentsByUserId(int id)
        {
            var userbycomments = await _repository.GetListAsync(i => i.AppUserId == id);
            if(userbycomments == null)
            {
                return new List<CommentDTO>();
            }
            
            var commentsDto = _mapper.Map<List<CommentDTO>>(userbycomments);

            return commentsDto;
        }
    }
}