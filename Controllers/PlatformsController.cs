using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")] //Define Route
    //[ApiController] //Decoration, Provides out of the box behaviour for API
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformsController> _logger;

        public PlatformsController(IPlatformRepo repository, IMapper mapper, ILogger<PlatformsController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ??  throw new ArgumentNullException(nameof(mapper));
            _logger = logger ??  throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PlatformReadDto>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
           _logger.LogInformation("---> Getting Platforms ...");
           var platformItems = _repository.GetAllPlatforms();
           //Maps to destination object from source object.
           return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name="GetPlatformById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PlatformReadDto), (int)HttpStatusCode.OK)]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _repository.GetPlatformById(id);
            if(platformItem == null)
            {
                _logger.LogError($"Platform with id: {id}, not found");
                return NotFound();
            }
            return Ok(_mapper.Map<PlatformReadDto>(platformItem));
        }

        [HttpPost]
        [ProducesResponseType(typeof(PlatformReadDto), (int)HttpStatusCode.Created)]
        public ActionResult<PlatformReadDto> CreatePlatform([FromBody]PlatformCreateDto platformCreateDto)
        {
            //Map default platform to CreatedDto
            var createdPlatform = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(createdPlatform);
            _repository.SaveChanges();

            //Map createdPlatform to readPlatform
            //Return ReadDto
            var platformReadDto = _mapper.Map<PlatformReadDto>(createdPlatform);
            return CreatedAtRoute("GetPlatformById", new {Id = platformReadDto.Id}, platformReadDto);
        }

        [HttpDelete("{id}")]
        public ActionResult<PlatformReadDto> DeletePlatform(int id)
        {
            var deletedPlatform = _repository.DeletePlatform(id);
            _logger.LogInformation($"Platform with id; {id} deleted successfully!");
            return Ok(_mapper.Map<PlatformReadDto>(deletedPlatform));
        }
    }
}