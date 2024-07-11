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
    }
}
