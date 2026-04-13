using AutoMapper;
using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;
using BlogApi.DataAccess.Abstract;
using BlogApi.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BlogApi.Business.Concrete;

namespace BlogApi2.Controller
{
    [ApiController,Authorize]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly IdentityClaimService _ıdentityClaimService;
        public TagController(ITagService tagService,IdentityClaimService ıdentityClaimService)
        {
           _tagService = tagService;
           _ıdentityClaimService = ıdentityClaimService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>GetTagByIdAsync(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if(tag == null)
            {
                return NotFound("tag bulunamadı");
            }

            return Ok(tag);
        }

        [HttpGet("posts/{id}/tags")]
        public async Task<IActionResult>GetPostTags(int id,[FromQuery]int page = 1,[FromQuery]int pageSize = 10)
        {
            var posttags = await _tagService.GetPagedTagsByPostId(id,page,pageSize);
            if(posttags == null)
            {
                return NotFound();
            }

            return Ok(posttags);
        }

        [HttpGet("post/{id}/tags")]
        public async Task<IActionResult>GetTagsByPostId(int id)
        {
            var gettagsbypostId = await _tagService.GetTagsByPostId(id);
            if(gettagsbypostId == null)
            {
                return NotFound();
            }

            return Ok(gettagsbypostId); 
        }

        [HttpPost]
        public async Task<IActionResult>CreateTag(TagDTO request)
        {
            var userId = _ıdentityClaimService.FindUserId();
                   
            var tag = await _tagService.CreateTagAsync(request);
            if(tag == false)
            {
                return NotFound("Bir post'a 6 taneden fazla tag oluşturamazsın");
            }

            return StatusCode(201,"Yorum oluşturuldu");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateTag(int id,TagDTO request)
        {

            var tag = await _tagService.UpdateTagAsync(id,request);
            if(tag == false)
            {
                return NotFound();
            }

            return NoContent();
        }        
    }
}
