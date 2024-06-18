using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Rater.Business.Services.Interfaces;
using Rater.Domain.DataTransferObjects.SpaceDto;
using Rater.Domain.DataTransferObjects.UserDto;

namespace Rater.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpaceController : ControllerBase
    {

        private readonly ISpaceService _service;

        public SpaceController(ISpaceService service)
        {
            _service = service;
        }

        [HttpPost("CreateSpace")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<SpaceResponseDto>> AddSpace(GrandSpaceRequestDto request)
        {
            var value  = await _service.AddSpace(request);
            return value;

        }


        [HttpGet("GetAllSpaces")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<List<SpaceResponseDto>>> GetAllSpaces()
        {
            var value = await _service.GetAllSpaces();
            return Ok(value);

        }
    }
}
