using AutoMapper;
using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;
using BlogApi.DataAccess.Abstract;
using BlogApi.Business.Abstract;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;

namespace BlogApi.Business.Concrete
{
    public class PostService : IPostService
    {
        private readonly IGenericRepository<Post> _repository;
        private readonly IMapper _mapper;
        
        public PostService(IGenericRepository<Post> repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> CreatePostAsync(int userId, PostDTO request)
        {
            var today = DateTime.UtcNow.Date;//Date saat dkları sıfırlar.
            var tomorrow = today.AddDays(1);
            
            var count = await _repository.CountAsync(x =>
                x.AppUserId == userId &&
                x.CreateAt >= today &&
                x.CreateAt < tomorrow);
            
            if(count >= 5)
            {
                return false;
            }

            var post = _mapper.Map<Post>(request);//Yeni entity oluşturur,,Mappler ,sonra ekler
            post.CreateAt = DateTime.UtcNow;
            post.AppUserId = userId;

            await _repository.AddAsync(post);
            int saveResult = await _repository.SaveAsync();

            if(saveResult == 1)
                return true;
            else
                return false;
        }

        public async Task<bool> DeletePostAsync(int id,int userId)
        {
            var post = await _repository.GetAsync(i =>i.PostId == id);
            if(post == null)
            {
                return false;
            }
            if(post.AppUserId != userId)
            {
                throw new Exception("Bu postu silmeye yetkiniz yoktur.!");
            }

            _repository.Remove(post);

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

        public async Task<PostDTO?> GetPostByIdAsync(int id)
        {
            
            var post = await _repository.GetAsync(p => p.PostId == id);
            if(post == null)
            {
                return null;
            }
            
            return _mapper.Map<PostDTO>(post);
        }

        public async Task<List<PostDTO>> GetPostsByTagId(int id)
        {
            var getpostsbytag = await _repository.GetListAsync(i => i.Tags.Any(i => i.TagId == id));

            var getpostsbytagDto = _mapper.Map<List<PostDTO>>(getpostsbytag);

            return getpostsbytagDto;
        }

        public async Task<List<PostDTO>> GetPostsByUserId(int id)
        {
            var getspostsbyuser = await _repository.GetListAsync(i => i.AppUserId == id);
            if(getspostsbyuser == null)
            {
                return null;
            }
            
            var getspostsbyuserdto = _mapper.Map<List<PostDTO>>(getspostsbyuser);
            
            return getspostsbyuserdto;
        }

        public async Task<PagedDTO<PostDTO>> GetPagedTagPosts(int id, int page, int pageSize)
        {
            var tagposts = await _repository.GetPagedAsync(page,pageSize,i => i.Tags.Any(i =>i.TagId == id));

            var result = new PagedDTO<PostDTO>
            {
                Items = _mapper.Map<List<PostDTO>>(tagposts.Items),
                TotalCount = tagposts.TotalCount,
                Page = tagposts.Page,
                PageSize = tagposts.PageSize
            };

            return result;
        }

        public async Task<bool> UpdatePostAsync(int id, PostDTO dto,int userId)
        {
            
            var post = await _repository.GetAsync(i => i.PostId == id);
            if(post == null)
            {
                return false;
            }
            
            if(post.AppUserId != userId)
            {
                throw new Exception("Bu postu güncellemeye yetkiniz yoktur!");
            }

            //post.AppUserId == userId

            _mapper.Map(dto,post);
            post.UpdateAt = DateTime.UtcNow;
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
        
        public async Task<PagedDTO<PostDTO>> GetPagedUserPosts(int id, int page, int pageSize)
        {
            var userposts = await _repository.GetPagedAsync(page, pageSize, p => p.AppUserId == id);

            var result = new PagedDTO<PostDTO>
            {
                Items = _mapper.Map<List<PostDTO>>(userposts.Items),
                TotalCount = userposts.TotalCount,
                Page = userposts.Page,
                PageSize = userposts.PageSize
            };

            return result;
        }

        public async Task<PagedDTO<ListPostDTO>> GetAllPosts(int page,int pageSize)
        {
            var posts = await _repository.GetPagedAsync(page, pageSize, filter: null, 
                p => p.Comments, 
                p => p.AppUser,
                p => p.PostLikes);
            
            var result = new PagedDTO<ListPostDTO>
            {
                Items = posts.Items.Select(p => new ListPostDTO
                {
                    UserId = p.AppUserId,
                    Title = p.Title,
                    TotalComment = p.Comments.Count,
                    TotalLike = p.PostLikes.Count,
                    UserName = p.AppUser.UserName,
                    CreatedAt = p.CreateAt
                }).ToList(),
                Page = posts.Page,
                PageSize = posts.PageSize,
                TotalCount = posts.TotalCount
            };

            return result;
        }
    }
}
