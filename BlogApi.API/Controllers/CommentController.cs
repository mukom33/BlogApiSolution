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

        [HttpGet("{id}")]
        public async Task<IActionResult>GetCommentByIdAsync (int id )
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if(comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpGet("post/{id}/comments")]
        public async Task<IActionResult>GetCommentsByPostId(int id)
        {
            var commentsbypostId = await _commentService.GetCommentsByPostId(id);
            if(commentsbypostId == null)
            {
                return NotFound();
            }

            return Ok(commentsbypostId);
        }

        [HttpGet("posts/{id}/comments")]
        public async Task<IActionResult>GetPagedCommentsByPostId(int id,[FromQuery]int pageSize = 10,[FromQuery]int page = 1)
        {
            var postcomments = await _commentService.GetPagedCommentsByPostId(id,page,pageSize);
            if(postcomments == null)
            {
                return NotFound();
            }

            return Ok(postcomments);
        }

        [HttpGet("AppUsers/{id}/comments")]
        public async Task<IActionResult>GetPagedCommentsByUserId(int id,[FromQuery]int pageSize = 10,[FromQuery]int page = 1)
        {
            var usercomments = await _commentService.GetPagedCommentsByUserId(id,page,pageSize);
            if(usercomments == null)
            {
                return NotFound();
            }
            
            return Ok(usercomments);
        }

        [HttpGet("AppUser/{id}/comments")]
        public async Task<IActionResult>GetCommentsByUserId(int id)
        {
            var getcommentsbyuser = await _commentService.GetCommentsByUserId(id);
            if(getcommentsbyuser == null)
            {
                return NotFound();
            }

            return Ok(getcommentsbyuser);
        }

        [HttpPost]
        public async Task<IActionResult>CreateComment(CommentDTO dto)
        {
            var userId = _ıdentityclaimService.FindUserId();
                
            var result = await _commentService.CreateAsync(userId,dto);
            if(result == false)
            {
                return NotFound("Bir kullanıcı 2 den fazla yorum yapamaz");
            }
            
            return StatusCode(201,"Yorum oluşturuldu");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateComment(int id,CommentDTO dto)
        {
            var userId = _ıdentityclaimService.FindUserId();
            var comment = await _commentService.UpdateAtAsync(id,dto,userId);
            if(comment == false)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteComment(int id)
        { 
            var userId = _ıdentityclaimService.FindUserId();

            var comment = await _commentService.DeletedAsync(id,userId);
            if(comment == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}