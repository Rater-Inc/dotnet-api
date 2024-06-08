using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rater.Business.Services.Interfaces;
using Rater.Domain.DataTransferObjects.UserDto;

namespace Rater.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserResponseDto>> CreateUser(UserRequestDto request)
        {
            var value = await _service.CreateUser(request);
            return Ok(value);   

        }
    }
}
