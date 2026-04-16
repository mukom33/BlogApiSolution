using AutoMapper;
using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;
using BlogApi.DataAccess.Abstract;
using BlogApi.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BlogApi.Business.Concrete;
using BlogApi.Business.Wrappers;

namespace BlogApi.API.Controllers
{
    [ApiController,Authorize]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IdentityClaimService _ıdentityclaimService;
        public CommentController(ICommentService commentService,IdentityClaimService ıdentityClaimService)
        {
            _commentService = commentService;
            _ıdentityclaimService = ıdentityClaimService;
        }

       /* [HttpGet("{id}")]
        public async Task<IActionResult>GetCommentByIdAsync (int id )
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if(comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }
*/
       /* [HttpGet("post/{id}")]
        public async Task<IActionResult>GetCommentsByPostId(int id)
        {
            var commentsbypostId = await _commentService.GetCommentsByPostId(id);
            if(commentsbypostId == null)
            {
                return NotFound();
            }

            return Ok(commentsbypostId);
        }
        */

        [HttpGet("post/{postId}")]
        public async Task<IActionResult>GetPagedCommentsByPostId(int postId,[FromQuery]int pageSize = 10,[FromQuery]int page = 1)
        {
            var postcomments = await _commentService.GetPagedCommentsByPostId(postId,page,pageSize);
            
            if(!postcomments.Success)
            {
                return NotFound(postcomments);
            }

            return Ok(postcomments);
        }

        [HttpPost]
        public async Task<IActionResult>CreateComment(int PostId,CommentDTO dto)
        {
            var userId = _ıdentityclaimService.FindUserId();
                
            var result = await _commentService.CreateAsync(PostId,userId,dto);
            if(!result.Success)
            {
                return NotFound(result);
            }
            
            return CreatedAtAction(nameof(CreateComment),result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateComment(int id,CommentDTO dto)
        {
            var userId = _ıdentityclaimService.FindUserId();
            if(userId == null)
            {
                return BadRequest();
            }
            var comment = await _commentService.UpdateAtAsync(id,dto,userId);
            if(!comment.Success)
            {
                return NotFound(comment);
            }
            else
            {
                return Ok(comment);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteComment(int id)
        { 
            var userId = _ıdentityclaimService.FindUserId();

            var comment = await _commentService.DeletedAsync(id,userId);
            if(!comment.Success)
            {
                return NotFound(comment);
            }

            return Ok(comment);
        }
    }
}