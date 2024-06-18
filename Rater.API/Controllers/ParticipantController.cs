using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Rater.Business.Services.Interfaces;
using Rater.Domain.DataTransferObjects.ParticipantDto;

namespace Rater.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _service;
        public ParticipantController(IParticipantService service)
        {
            _service = service;
        }


        [HttpGet("GetParticipants")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<List<ParticipantResponseDto>>> GetParticipants(int space_id)
        {

            try
            {
                var value = await _service.GetParticipants(space_id);
                return Ok(value);
            }
            catch (Exception ex) { 
            
                return BadRequest(ex.Message);
            }

            
        }
    }
}
