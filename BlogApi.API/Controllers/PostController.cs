using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;
using BlogApi.DataAccess.Abstract;
using BlogApi.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BlogApi.Business.Concrete;
using Microsoft.IdentityModel.Tokens;
using System.Xml;

namespace BlogApi.API.Controllers 
{
    [ApiController,Authorize]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private IPostService _postService;
        private IdentityClaimService _ıdentityClaimservice;

        public PostController(IPostService postService,IdentityClaimService ıdentityClaimService)
        {
            _postService = postService;
            _ıdentityClaimservice = ıdentityClaimService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPostByPaged([FromQuery]int page=1,[FromQuery]int PageSize = 10)
        {
            var posts = await _postService.GetAllPosts(page,PageSize);

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>GetPostByIdAsync(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if(post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpGet("AppUsers/{id}/posts")]
        public async Task<IActionResult>GetUserPosts(int id,[FromQuery]int page=1,[FromQuery]int pageSize = 10)
        {
            var userposts = await _postService.GetPagedUserPosts(id,page,pageSize);
            if(userposts == null)
            {
                return BadRequest();
            }
            
            return Ok(userposts);
        }

        [HttpGet("tags/{id}/posts")]
        public async Task<IActionResult>GetTagPosts(int id,[FromQuery]int page=1,[FromQuery]int pageSize = 10)
        {
            var tagposts = await _postService.GetPagedTagPosts(id,page,pageSize);
            if(tagposts == null)
            {
                return NotFound();
            }

            return Ok(tagposts);
        }

        [HttpGet("AppUser/{id}/posts")]
        public async Task<IActionResult>GetPostsByAppUserId(int id)
        {
            var getpostsbyuser = await _postService.GetPostsByUserId(id);
            if(getpostsbyuser == null)
            {
                return NotFound();
            }

            return Ok(getpostsbyuser);
        } 

        [HttpGet("Tag/{id}/posts")]
        public async Task<IActionResult>GetPostsByTagId(int id)
        {
           var getpostsbytag = await _postService.GetPostsByTagId(id);
           if(getpostsbytag == null)
            {
                return NotFound();
            }

            return Ok(getpostsbytag);
        }
        
        [HttpPost]
        public async Task<IActionResult>CreatePost(PostDTO request)
        {
            var userId = _ıdentityClaimservice.FindUserId();

            var result = await _postService.CreatePostAsync(userId, request);

            if (result == false)
            {
                return NotFound("Bir kullanıcı bir günde en fazla 5 post oluşturabilir.");
            }

            return StatusCode(201,"Yorum oluşturuldu");    
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult>UpdatePost(int id, PostDTO request)
        {
            var userId = _ıdentityClaimservice.FindUserId();
            
            var post = await _postService.UpdatePostAsync(id,request,userId);
            if(post == false)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeletePostAsync(int id)
        {
            var userId = _ıdentityClaimservice.FindUserId();

            var post = await _postService.DeletePostAsync(id,userId);
            if(post == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
