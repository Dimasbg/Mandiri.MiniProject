using Mandiri.MiniProject.Utilities.Base;
using Microsoft.AspNetCore.Mvc;
using NotificationApp.Api.Models.NotificationModels;
using NotificationApp.Domain.NotificationService;
using System.Net;

namespace NotificationApp.Api.Controllers
{
    [ApiController]
    [Route("V1/[controller]")]
    public class NotificationController:ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotificationApiModel p, CancellationToken c)
        {
            try
            {
                var result = await _notificationService.Create(p.ToDao(), c);
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Read([FromRoute] int id, CancellationToken c)
        {
            try
            {
                var result = await _notificationService.Read(id, c);
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
        public async Task<IActionResult> List([FromQuery] NotificationListParamApiModel p, CancellationToken c)
        {
            try
            {
                var result = await _notificationService.List(p, c);
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
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] NotificationApiModel p, CancellationToken c)
        {
            try
            {
                var result = await _notificationService.Update(p.ToDao(id), c);
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken c)
        {
            try
            {
                await _notificationService.Delete(id, c);
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
