using System.Security.Cryptography.X509Certificates;
using BlogApi.Domain.Entities;
using BlogApi.DataAccess.Abstract;
using BlogApi.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogApi.Business.Concrete;

namespace BlogApi.API.Controllers
{
    [ApiController,Authorize]
    [Route("api/[controller]")]
    public class PostLikeController : ControllerBase
    {
        private readonly IdentityClaimService _ıdentityClaimService;
        private readonly IPostLikeService _postlikeService;
        public PostLikeController(IPostLikeService postLikeService,IdentityClaimService ıdentityClaimService)
        {
            _postlikeService = postLikeService;
            _ıdentityClaimService = ıdentityClaimService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>GetPostLike(int id)
        {
            var postlike = await _postlikeService.GetPostLikeById(id);
            if(postlike == null)
            {
                return NotFound();
            }

            return Ok(postlike);
        }

        [HttpGet("Post/{id}/postlikes")]
        public async Task<IActionResult>GetPostLikeByPostId(int id)
        {
            var postlikebypostId = await _postlikeService.GetPostLikeByPostId(id);
            if(postlikebypostId == null)
            {
                return NotFound();
            }

            return Ok(postlikebypostId);
        }

        [HttpGet("posts/{id}/postlikes")]
        public async Task<IActionResult>GetPostPostLikes(int id,[FromQuery]int page,[FromQuery]int pageSize = 10)
        {
            var postpostlikes = await _postlikeService.GetPostPostLikes(id,page,pageSize);
            if(postpostlikes == null)
            {
                return NotFound();
            }

            return Ok(postpostlikes);
        }

        [HttpGet("AppUsers/{id}/postlikes")]
        public async Task<IActionResult>GetUserPostLikes(int id,[FromQuery]int page = 1,[FromQuery]int pageSize = 10)
        {
            var userpostlikes = await _postlikeService.GetUserPostlikes(id,page,pageSize);
            if(userpostlikes == null)
            {
                return NotFound();
            }
            
            return Ok(userpostlikes);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeletePostLike(int id)
        {
            var userId = _ıdentityClaimService.FindUserId();

            var postlike = await _postlikeService.DeletePostLike(id,userId);
            if(postlike == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}