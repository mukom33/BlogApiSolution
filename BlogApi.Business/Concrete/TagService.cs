using AutoMapper;
using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;
using BlogApi.DataAccess.Abstract;
using BlogApi.Business.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Business.Concrete
{
    public class TagService : ITagService
    {
        private readonly IGenericRepository<Tag> _repository;
        private readonly IMapper _mapper;
        public TagService(IGenericRepository<Tag> repository,IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<bool> CreateTagAsync(TagDTO request)
        {
           var tagCount = await _repository.CountAsync(x => x.Posts.Any(p => p.PostId == request.PostId));           
            if(tagCount >= 6)
            {
                return false;
            }
            
            var tag = _mapper.Map<Tag>(request);
            tag.CreatedAt = DateTime.UtcNow;
            tag.UpdatedAt = null;
            tag.DeletedAt = null;
            
            

            await _repository.AddAsync(tag);
            await _repository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            var tag = await _repository.GetAsync(i => i.TagId == id);
            if(tag == null)
            {
                return false;
            }

            _repository.Remove(tag);
            await _repository.SaveAsync();
            
            return true;
        }

        public async Task<TagDTO> GetTagByIdAsync(int id)
        {
            var tag = await _repository.GetAsync(i => i.TagId == id);
            if(tag == null)
            {
                return null;
            }

            return _mapper.Map<TagDTO>(tag);           
        }

        public async Task<bool> UpdateTagAsync(int id, TagDTO request)
        {
            var tag = await _repository.GetAsync(i => i.TagId == id);
            if(tag == null)
            {
                return false;
            }

            _mapper.Map(tag,request);
            tag.UpdatedAt = DateTime.UtcNow;

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

        public async Task<PagedDTO<TagDTO>> GetPagedTagsByPostId(int id, int page, int pageSize)
        {
            var postTags = await _repository.GetPagedAsync(page,pageSize,t => t.Posts.Any(i => i.PostId == id));

            var result = new PagedDTO<TagDTO>
            {
                Items = _mapper.Map<List<TagDTO>>(postTags),
                TotalCount = postTags.TotalCount,
                Page = postTags.Page,
                PageSize = postTags.PageSize
            };

            return result;
        }

        public async Task<List<TagDTO>> GetTagsByPostId(int id)
        {
            var tagsbypostId = await _repository.GetListAsync(i => i.Posts.Any(i => i.PostId == id));
            
            var tagsbypostDto = _mapper.Map<List<TagDTO>>(tagsbypostId);

            return tagsbypostDto;
        }
    }
}