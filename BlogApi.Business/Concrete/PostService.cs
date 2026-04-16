using AutoMapper;
using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;
using BlogApi.DataAccess.Abstract;
using BlogApi.Business.Abstract;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using BlogApi.Business.Wrappers;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogApi.Business.Concrete
{
    public class PostService : IPostService
    {
        private readonly IGenericRepository<Post> _repository;
        private readonly IGenericRepository<AppUser> _userRepository;
        private readonly IMapper _mapper;
        
        public PostService(IGenericRepository<Post> repository,IMapper mapper,IGenericRepository<AppUser> userRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<PostDTO>> CreatePostAsync(int userId, PostDTO request)
        {
            try
            {
                var today = DateTime.UtcNow.Date;//Date saat dkları sıfırlar.
                var tomorrow = today.AddDays(1);
                
                var count = await _repository.CountAsync(x =>
                    x.AppUserId == userId &&
                    x.CreateAt >= today &&
                    x.CreateAt < tomorrow);
                
                if(count >= 5)
                {
                    return ApiResponse<PostDTO>.FailResponse("Birgünde En Fazla 5 Post Atabilirsiniz.");
                }

                var post = _mapper.Map<Post>(request);//Yeni entity oluşturur,,Mappler ,sonra ekler
                post.CreateAt = DateTime.UtcNow;
                post.AppUserId = userId;

                await _repository.AddAsync(post);
                int saveResult = await _repository.SaveAsync();

                var postDto = _mapper.Map<PostDTO>(post);

                if(saveResult == 1)
                    return ApiResponse<PostDTO>.SuccessResponse(postDto,"Post Oluşturuldu");
                else
                    return ApiResponse<PostDTO>.FailResponse("Post Oluşturulmadı");
            }
            catch(Exception ex)
            {
                return ApiResponse<PostDTO>.FailResponse(ex.Message);
            }
        }

        public async Task<ApiResponse<Post>> DeletePostAsync(int id,int userId)
        {
            try
            {
                var post = await _repository.GetAsync(i =>i.PostId == id);
                
                if(post == null)
                {
                    return ApiResponse<Post>.FailResponse("Böyle bir post bulunamadı");
                }
                if(post.AppUserId != userId)
                {
                    return ApiResponse<Post>.FailResponse("Bu Postu silmeye yetkiniz yoktur");
                }

                _repository.Remove(post);

                await _repository.SaveAsync();

                return ApiResponse<Post>.SuccessResponse(post,"Post Başarıyla Silindi");
            }
            
            catch (Exception ex)
            {
                return ApiResponse<Post>.FailResponse(ex.Message);
            }

           
        }

        public async Task<ApiResponse<List<ListPostDTO>>> GetPostByIdAsync(int id)
        {
            try
            {
                var post = await _repository.GetListAsync(
                    p => p.PostId == id,
                    p=>p.AppUser,
                    p=>p.Comments,
                    p=>p.PostLikes);

                if(post == null || !post.Any())
                {
                    return ApiResponse<List<ListPostDTO>>.FailResponse("Böyle bir Post Bulunamadı");
                }

                var postdto = post.Select(p=> new ListPostDTO
                {
                    UserId = p.AppUserId,
                    UserName = p.AppUser.UserName,
                    Title = p.Title,
                    CreatedAt = p.CreateAt,
                    TotalComment = p.Comments.Count,
                    TotalLike = p.PostLikes.Count,
                    PostId = p.PostId
                }).ToList();

                return ApiResponse<List<ListPostDTO>>.SuccessResponse(postdto);
            }
            catch(Exception ex)
            {
                return ApiResponse<List<ListPostDTO>>.FailResponse(ex.Message);
            }
        }

      

        // public async Task<List<ListPostDTO>> GetPostsByUserId(int id)
        // {
        //     var getspostsbyuser = await _repository.GetListAsync(i => i.AppUserId == id );
        //     if(getspostsbyuser == null)
        //     {
        //         return null;
        //     }
            
        //     var getspostsbyuserdto = new List<ListPostDTO>
        //     {
        //         getspostsbyuser.Select(p => new ListPostDTO
        //         {
        //             UserId = p.AppUserId,
        //             Title = p.Title,
        //             TotalComment = p.Comments.Count,
        //             UserName = p.AppUser.UserName,

        //         }).ToList()
        //     };
            
        //     return getspostsbyuserdto;
        // }

       

        public async Task<ApiResponse<PostDTO>> UpdatePostAsync(int id, PostDTO dto,int userId)
        {
            try
            {     
                var post = await _repository.GetAsync(i => i.PostId == id);
                if(post == null)
                {
                    return ApiResponse<PostDTO>.FailResponse("Böyle bir Post bulunamadı");
                }

                if(post.AppUserId != userId)
                {
                    return ApiResponse<PostDTO>.FailResponse("Bu Postu Güncellemeye Yetkiniz Yoktur");
                }

                _mapper.Map(dto,post);
                post.UpdateAt = DateTime.UtcNow;

                var result = await _repository.SaveAsync();

                if(result == 0)
                {
                    return ApiResponse<PostDTO>.FailResponse("Güncelleme başarısız");
                }

                var postdto = _mapper.Map<PostDTO>(post);

                return ApiResponse<PostDTO>.SuccessResponse(postdto,"Güncelleme Başarıyla yapıldı");
            }
            catch (Exception ex )
            {
                return ApiResponse<PostDTO>.FailResponse(ex.Message);
            }
        }
        
        public async Task<ApiResponse<PagedDTO<ListPostDTO>>> GetPagedUserPosts(int id, int page, int pageSize)
        {
            try
            {
            var userCount = await _userRepository.CountAsync(u => u.Id == id);
            bool exists = userCount > 0;
            if (!exists)
            {
                return ApiResponse<PagedDTO<ListPostDTO>>.FailResponse("Böyle bir Kullanıcı bulunamadı");
            }

            var userposts = await _repository.GetPagedAsync(
                page, pageSize,
                p => p.AppUserId == id,
                p => p.AppUser,p => p.Comments,
                p=>p.PostLikes
            );

            if (userposts.Items == null || !userposts.Items.Any())
            {
                return ApiResponse<PagedDTO<ListPostDTO>>.FailResponse("Bu  Kullanıcının bir  Postu Bulunamadı");
            }

            var result = new PagedDTO<ListPostDTO>
            {
                Items = userposts.Items.Select(p => new ListPostDTO
                {
                    UserId = p.AppUserId,
                    Title = p.Title,
                    TotalComment = p.Comments?.Count ?? 0,
                    TotalLike = p.PostLikes?.Count ?? 0,
                    UserName = p.AppUser?.UserName ?? string.Empty,
                    CreatedAt = p.CreateAt,
                    PostId = p.PostId
                }).ToList(),
                TotalCount = userposts?.TotalCount ?? 0,
                Page = userposts?.Page ?? 0,
                PageSize = userposts.PageSize
            };

            return ApiResponse<PagedDTO<ListPostDTO>>.SuccessResponse(result,$"{id} id/'li  kullanıcının Postları Başarıyla Getirildi");
            }
            catch(Exception ex)
            {
                return ApiResponse<PagedDTO<ListPostDTO>>.FailResponse(ex.Message);
            }
        }

        public async Task<ApiResponse<PagedDTO<ListPostDTO>>>GetAllPosts(int page,int pageSize)
        {
            var posts = await _repository.GetPagedAsync(page, pageSize, filter: null, 
                p => p.Comments, 
                p => p.AppUser,
                p => p.PostLikes);

            if(posts.Items == null || !posts.Items.Any())
            {
                return ApiResponse<PagedDTO<ListPostDTO>>.FailResponse("Tüm Postlar Listelenirken Hata Oluştu");
            }
            
            var result = new PagedDTO<ListPostDTO>
            {
                Items = posts.Items.Select(p => new ListPostDTO
                {
                    UserId = p.AppUserId,
                    Title = p.Title,
                    TotalComment = p.Comments.Count,
                    TotalLike = p.PostLikes.Count,
                    UserName = p.AppUser.UserName,
                    CreatedAt = p.CreateAt,
                    PostId = p.PostId
                })
                .ToList(),
                Page = posts.Page,
                PageSize = posts.PageSize,
                TotalCount = posts.TotalCount
            };

            return ApiResponse<PagedDTO<ListPostDTO>>.SuccessResponse(result,"Tüm Postlar Başarıyla Listelendi");
        }

    }
}
