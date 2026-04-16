using AutoMapper;
using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;
using BlogApi.DataAccess.Abstract;
using BlogApi.Business.Abstract;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using BlogApi.Business.Wrappers;
using Microsoft.VisualBasic;

namespace BlogApi.Business.Concrete
{
    public class CommentService : ICommentService
    {
        private readonly IGenericRepository<Comment> _repository;
        private readonly IGenericRepository<Post> _postRepository;
        private readonly IMapper _mapper;
        public CommentService(IGenericRepository<Comment> repository,IMapper mapper,IGenericRepository<Post> postrepository)
        {
            _repository = repository;
            _mapper = mapper;
            _postRepository = postrepository;
        }

       /* public async Task<CommentDTO> GetCommentByIdAsync(int id)
        {
            var comment = await _repository.GetAsync(i => i.CommentId == id);
            if(comment == null)
            {
                return null;
            }    

            return _mapper.Map<CommentDTO>(comment);    
        }
*/
        public async Task<ApiResponse<CommentDTO>> CreateAsync(int PostId,int userId,CommentDTO request)
        {
            try
            {
                var postCount = await _postRepository.CountAsync(i => i.PostId == PostId);
                bool exists = postCount >0;
                if(!exists)
                {
                    return ApiResponse<CommentDTO>.FailResponse("Böyle bir Post yok");
                }

                var commentcount = await _repository.CountAsync(c => 
                    c.PostId == PostId&&
                    c.AppUserId == userId);

                if(commentcount >= 2)
                {
                    return ApiResponse<CommentDTO>.FailResponse("Bir posta 2'den fazla yorum yapamazsınız");
                }
                
                var comment = _mapper.Map<Comment>(request);
                comment.CreatedAt = DateTime.UtcNow;
                comment.AppUserId = userId;
                comment.PostId = PostId;

                await _repository.AddAsync(comment);
                var result = await _repository.SaveAsync();

                if (result == 0)
                {
                    return ApiResponse<CommentDTO>.FailResponse("işlem başarısız");
                }
                
                var commentDto= _mapper.Map<CommentDTO>(comment);
                
                return ApiResponse<CommentDTO>.SuccessResponse(commentDto);
            }
            catch(Exception ex)
            {
                return ApiResponse<CommentDTO>.FailResponse(ex.Message);
            }

            
        }

        public async Task<ApiResponse<Comment>> DeletedAsync(int id,int userId)
        {
            
            
            var commentCount = await _repository.CountAsync(i => i.CommentId == id);

            if(commentCount == 0)
            {
                return ApiResponse<Comment>.FailResponse("Böyle bir yorum bulunamadı");
            }

            var comment = await _repository.GetAsync(i => i.CommentId == id);

            if(comment.AppUserId != userId)
            {
                return ApiResponse<Comment>.FailResponse("Silme işlemini yapmaya yetkiniz yoktur.");
            }
            try
            {  
                _repository.Remove(comment);
            
                await _repository.SaveAsync();
            }    
           
            catch (Exception ex)
            {
                return ApiResponse<Comment>.FailResponse(ex.Message);
            }

            return ApiResponse<Comment>.SuccessResponse(comment,"Silme işlemi başarılı");         
        }

        public async  Task<ApiResponse<PagedDTO<ListCommentDTO>>>GetPagedCommentsByPostId(int id,int page,int pageSize)
        {
            try
            {
                var commentCount = await _postRepository.CountAsync(i => i.PostId == id);
                bool existPost = commentCount>0;

                if(!existPost)
                {
                    return ApiResponse<PagedDTO<ListCommentDTO>>.FailResponse("Böyle bir post bulunamadı");
                }

                var comment = await _repository.GetPagedAsync(
                    page,
                    pageSize,
                    i => i.PostId == id,
                    p => p.AppUser,
                    p => p.Post
                );

                var commentResult = new PagedDTO<ListCommentDTO>
                {
                    Items = comment.Items.Select(p => new ListCommentDTO
                    {
                        UserName = p.AppUser.UserName,
                        CreatedAt = p.CreatedAt,
                        Text = p.Text,
                        UserId = p.AppUserId,
                        CommentId = p.CommentId
                    }).ToList(),
                    TotalCount = comment.TotalCount,
                    Page = comment.Page,
                    PageSize = comment.PageSize
                };

                return ApiResponse<PagedDTO<ListCommentDTO>>.SuccessResponse(commentResult,"İşlem Başarılı");
            }
            catch(Exception ex)
            {
                return ApiResponse<PagedDTO<ListCommentDTO>>.FailResponse(ex.Message);
            }

        }

       /* public async Task<List<ListCommentDTO>> GetCommentsByPostId(int id)
        {
            var postbycomments = await _repository.GetListAsync(i => i.PostId == id);
            if(postbycomments == null)
            {
                return new List<ListCommentDTO>();
            }

            var commentsDto = postbycomments.Select(p => new ListCommentDTO
            {
                
            });
            
            return commentsDto;
        }
        */

        public async Task<ApiResponse<CommentDTO>> UpdateAtAsync(int id, CommentDTO request,int userId)
        {
            try
            {
                var commentCount = await _repository.CountAsync(i => i.CommentId == id);
                bool exits = commentCount > 0;

                if (!exits)
                {
                    return ApiResponse<CommentDTO>.FailResponse("Böyle bir Yorum bulunamadı");
                }

                var comment = await _repository.GetAsync(i => i.CommentId == id);

                if(comment == null)
                {
                    return ApiResponse<CommentDTO>.FailResponse("İşlem başarısız");
                }

                if(comment.AppUserId != userId)
                {
                    return ApiResponse<CommentDTO>.FailResponse("Bu yorumu Update etmeye yetkiniz yoktur");
                }

                _mapper.Map(request,comment);
                comment.UpdatedAt = DateTime.UtcNow;
                
                var result =await _repository.SaveAsync();

                if(result == 0)
                {
                    return ApiResponse<CommentDTO>.FailResponse("Güncelleme Başarısız");
                }
            
                var commentDto = _mapper.Map<CommentDTO>(comment);

                return ApiResponse<CommentDTO>.SuccessResponse(commentDto,"Güncelleme Başarıyla Gerçekleşti");
            }

            catch (Exception ex)
            {
                return ApiResponse<CommentDTO>.FailResponse(ex.Message);
            }
        }
    }
}