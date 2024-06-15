using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<List<RatingForMetricResponseDto>>> AddRatings(RatingRequestDto request)
        {

            try
            {
                var value = await _service.AddRatings(request);
                return value;
            }
            catch (UnauthorizedAccessException exes)
            {
                return Unauthorized(exes.Message);
            }
            catch (Exception ex) { 

                return BadRequest(ex.Message);
            }

            

            
        } 



    }
}
