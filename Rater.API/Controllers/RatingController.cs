using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Rater.Business.Services.Interfaces;
using Rater.Domain.DataTransferObjects.RatingDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Rater.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {

        private readonly IRatingService _service;
        public RatingController(IRatingService service)
        {
            _service = service;
        }


        [HttpPost , Authorize(Policy = "SpaceIdentify")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<RatingResponseDto>> AddRatings(RatingRequestDto request)
        {

            try
            {
                var value = await _service.AddRatings(request);
                return value;
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (Exception ex) {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            

            
        } 



    }
}
