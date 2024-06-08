﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rater.Business.Services.Interfaces;
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

        [HttpPost("CreateSpace")]
        public async Task<ActionResult<SpaceResponseDto>> CreateSpace(SpaceRequestDto request)
        {
            var value = await _service.CreateSpace(request);
            return Ok(value);
        }


        [HttpGet("GetAllSpaces")]
        public async Task<ActionResult<List<SpaceResponseDto>>> GetAllSpaces()
        {
            var value = await _service.GetAllSpaces();
            return Ok(value);

        }
    }
}
