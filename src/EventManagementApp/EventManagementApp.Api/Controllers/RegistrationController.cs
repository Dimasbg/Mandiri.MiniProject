using EventManagementApp.Api.Models.EventModels;
using EventManagementApp.Api.Models.RegistrationModels;
using EventManagementApp.Domain.EventService;
using EventManagementApp.Domain.RegistrationService;
using Mandiri.MiniProject.Utilities.Base;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventManagementApp.Api.Controllers
{
    [ApiController]
    [Route("V1/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegistrationApiModel p, CancellationToken c)
        {
            try
            {
                var result = await _registrationService.Create(p.ToDao(), c);
                return Ok(result);
            }
            catch (ApiBadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (DataNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (DomainLayerException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Read([FromRoute] int id, CancellationToken c, [FromQuery] bool isIncludeEvent = false)
        {
            try
            {
                var result = await _registrationService.Read(id, c, isIncludeEvent);
                return Ok(result);
            }
            catch (DataNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (DomainLayerException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] RegistrationListParamApiModel p, CancellationToken c)
        {
            try
            {
                var result = await _registrationService.List(p, c);
                return Ok(result);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (DomainLayerException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] RegistrationApiModel p, CancellationToken c)
        {
            try
            {
                var result = await _registrationService.Update(p.ToDao(id), c);
                return Ok(result);
            }
            catch (ApiBadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (DataNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (DomainLayerException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken c)
        {
            try
            {
                await _registrationService.Delete(id, c);
                return Ok();
            }
            catch (DataNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (DomainLayerException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
