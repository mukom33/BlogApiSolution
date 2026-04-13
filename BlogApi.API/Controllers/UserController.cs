using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;
using BlogApi.Business.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
           _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult>CreatedUser(UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUser(dto);
            if(!result)
            {
                return BadRequest(new {message = "Kullanıcı oluşturulamadı"});
            }

            return StatusCode(201,new {message = "Kullanıcı oluşturuldu"});
        }

        [HttpPost("login")]
        public async Task<IActionResult>LoginUser(LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _userService.LoginAsync(dto);
            if(token == null)
            {
                return Unauthorized(new{message="Email veya şifre Hatalı"});
            }

            return Ok(new{ token });
        }
    }
}