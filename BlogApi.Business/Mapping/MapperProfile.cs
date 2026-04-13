using AutoMapper;
using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;

namespace BlogApi.Business.Mapping
{
    public class MapperProfile : Profile
    {
       public MapperProfile()
        {
            CreateMap<AppUser,UserDTO>().ReverseMap();
            CreateMap<AppUser,LoginDTO>().ReverseMap();
            CreateMap<Tag,TagDTO>().ReverseMap();
            CreateMap<Post,PostDTO>().ReverseMap();
            CreateMap<Comment,CommentDTO>().ReverseMap();
            CreateMap<PostLike,PostLikeDTO>().ReverseMap();
        }
    }
}