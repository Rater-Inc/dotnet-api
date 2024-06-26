﻿using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Rater.Business.Services.Interfaces;
using Rater.Domain.DataTransferObjects.ResultDto;
using Rater.Domain.DataTransferObjects.SpaceDto;
using Rater.Domain.DataTransferObjects.UserDto;

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

        [HttpPost("CreateSpace")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<SpaceResponseDto>> AddSpace(GrandSpaceRequestDto request)
        {
            var value  = await _service.AddSpace(request);
            return value;

        }

        [HttpPost("GetSpaceByLink"), Authorize(Policy = "SpaceIdentify")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<SpaceResponseDto>> GetSpace(string link)
        {
            try
            {
                var value = await _service.GetSpace(link);
                return Ok(value);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            } 
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            

        }

        [HttpPost("SpaceResults") , Authorize(Policy = "SpaceIdentify")]
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
                return Unauthorized(ex.Message);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            

        }
    }
}
