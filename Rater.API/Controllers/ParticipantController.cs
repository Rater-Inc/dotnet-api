using Microsoft.AspNetCore.Mvc;
using Rater.Business.Services.Interfaces;

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
    }
}
