using FeedBackApp.Api.Models.FeedBackModels;
using FeedBackApp.Domain.FeedbackService;
using Mandiri.MiniProject.Utilities.Base;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FeedBackApp.Api.Controllers
{
    [ApiController]
    [Route("V1/[controller]")]
    public class FeedBackController:ControllerBase
    {
        private readonly IFeedbackService _feedBackService;

        public FeedBackController(IFeedbackService feedbackService)
        {
            _feedBackService = feedbackService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeedBackApiModel p, CancellationToken c)
        {
            try
            {
                var result = await _feedBackService.Create(p.ToDao(), c);
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
                var result = await _feedBackService.Read(id, c);
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
        public async Task<IActionResult> List([FromQuery] FeedBackListParamApiModel p, CancellationToken c)
        {
            try
            {
                var result = await _feedBackService.List(p, c);
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
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] FeedBackApiModel p, CancellationToken c)
        {
            try
            {
                var result = await _feedBackService.Update(p.ToDao(id), c);
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
                await _feedBackService.Delete(id, c);
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
