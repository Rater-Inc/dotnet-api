using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rater.Business.Services.Interfaces;
using Rater.Domain.DataTransferObjects.MetricDto;

namespace Rater.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricController : ControllerBase
    {

        IMetricService _service;
        public MetricController(IMetricService service)
        {
            _service = service;
        }

        [HttpGet("GetSpaceMetrics")]
        public async Task<ActionResult<List<MetricResponseDto>>> GetSpaceMetrics (int space_id)
        {
            try
            {
                var value = await _service.GetMetrics(space_id);
                return Ok(value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            
        }

        [HttpPost("CreateMetrics")]
        public async Task<ActionResult<List<MetricResponseDto>>> CreateMetrics(List<MetricRequestDto> request)
        {
            var value = await _service.CreateMetrics(request);
            return Ok(value);

        }
    }
}
