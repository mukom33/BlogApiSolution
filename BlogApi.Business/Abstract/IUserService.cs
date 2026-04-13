using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;

namespace BlogApi.Business.Abstract
{
    public interface IUserService
    {
        Task<bool>CreateUser(UserDTO dto);
        Task<string>LoginAsync(LoginDTO dto);
    }
}