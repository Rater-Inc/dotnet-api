using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Rater.Business.Services.Interfaces;
using Rater.Domain.DataTransferObjects.AuthDto;

namespace Rater.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }


        [HttpPost]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<AuthResponseDto>> AuthSpace (string link,string password) {

            try
            {

                var value = await _service.AuthLobby(link, password);
                return Ok(value);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Invalid operation: " + ex.Message);
            }

            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }

        }
    }
}
