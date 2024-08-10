using Microsoft.AspNetCore.Mvc;
using Rater.Business.Services.Interfaces;

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
    }
}
