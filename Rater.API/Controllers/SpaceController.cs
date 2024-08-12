using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Rater.Business.Services.Interfaces;
using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.DataTransferObjects.ResultDto;
using Rater.Domain.DataTransferObjects.SpaceDto;

namespace Rater.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpaceController : ControllerBase
    {

        private readonly ISpaceService _service;

        public SpaceController(ISpaceService service)
        {
            _service = service;
        }

        [HttpPost("create-space")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<SpaceResponseDto>> AddSpace(GrandSpaceRequestDto request)
        {
            try
            {
                var value = await _service.AddSpace(request);
                return value;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        [HttpPost("get-space"), Authorize(Policy = "SpaceIdentify")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<SpaceResponseDto>> GetSpace(string link)
        {
            try
            {
                var value = await _service.GetSpace(link);
                return Ok(value);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        [HttpPost("space-result"), Authorize(Policy = "SpaceIdentify")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<GrandResultResponseDto>> GetSpaceResults(string link)
        {
            try
            {
                var value = await _service.GetSpaceResults(link);
                return Ok(value);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        //, Authorize(Policy = "SpaceIdentify")
        [HttpPost("add-ratings")]
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
                throw new InvalidOperationException(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
