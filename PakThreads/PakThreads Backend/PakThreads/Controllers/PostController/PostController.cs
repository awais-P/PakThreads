using Application.Interfaces.Post;
using Application.ViewModels.PostVm;
using Application.ViewModels.UserVm;
using Infrastructure.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PakThreads.Controllers.PostController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPost _postServices;
        public PostController(IPost postServices)
        {
            _postServices = postServices;
        }

        [Authorize]
        [HttpPost("CreatePost")]
        public IActionResult CreateUser(PostVm model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = _postServices.CreatePost(model);
            return Ok(result);
        }

        [HttpPost("GetPostData")]
        public IActionResult GetPostData(PostSearchVm model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = _postServices.GetAllPosts(model);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("GetUserPosts")]
        public IActionResult GetUserPosts(PostSearchVm model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = _postServices.GetUserPosts(model);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("DeletePost")]
        public IActionResult DeletePost(long postId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = _postServices.DeletePost(postId);
            return Ok(result);
        }

    }
}
