using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rater.Business.Services.Interfaces;
using Rater.Domain.DataTransferObjects.RatingDto;

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


        [HttpPost]
        public async Task<ActionResult<List<RatingForMetricResponseDto>>> AddRatings(RatingRequestDto request)
        {

            try
            {
                var value = await _service.AddRatings(request);
                return value;
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }

            
        } 



    }
}
