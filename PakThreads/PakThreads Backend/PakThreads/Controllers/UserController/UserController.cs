using Application.Interfaces.User;
using Application.ViewModels.CommonVm;
using Application.ViewModels.UserVm;
using Infrastructure.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PakThreads.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _UserService;
        public UserController(IUser UserService)
        {
            _UserService = UserService;
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser(UserVm user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = _UserService.CreateUser(user);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("SaveAndUpdateUserDetail")]
        public IActionResult SaveAndUpdateUserDetail(UserDataVm user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = _UserService.SaveAndUpdateUserDetail(user);
            return Ok(result);
        }

        [HttpPost("UserLogin")]
        public IActionResult LoginUser(LoginUserVm model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = _UserService.LoginUser(model);
            return Ok(result);
        }

        [HttpPost("GetUsers")]
        public IActionResult GetUsers(UserSearchVm model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = _UserService.GetUsers(model);

            return Ok(result);
        }

        [HttpPost("UnBlockUser")]
        public IActionResult UnBlockUser(long Id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = _UserService.UnBlockUser(Id);
            return Ok(result);
        }
        [HttpPost("BlockUser")]
        public IActionResult BlockUser(DeclineVm model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = _UserService.BlockUser(model);
            return Ok(result);
        }
        
        [HttpPost("DeleteUser")]
        public IActionResult DeleteUser(long Id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = _UserService.DeleteUser(Id);
            return Ok(result);
        }
        [HttpGet("UserCount")]
        public IActionResult GetUserCount()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = _UserService.GetUserCount();
            return Ok(result);
        }

        [Authorize]
        [HttpPost("SaveAndUpdateUserImage")]
        public IActionResult SaveAndUpdateUserImage(UserImageVm user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = _UserService.SaveAndUpdateUserImage(user);
            return Ok(result);
        }
    }
}
