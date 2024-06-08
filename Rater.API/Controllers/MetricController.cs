using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rater.Business.Services.Interfaces;

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

        [HttpGet("GetAllMetrics")]
        public async Task<ActionResult<List<Metric>>> GetAllMetrics ()
        {
            var value = await _service.GetMetrics();
            return Ok(value);
        }
    }
}
