using Mandiri.MiniProject.Utilities.Base;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TicketingApp.Api.Models.PurchaseModels;
using TicketingApp.Domain.PurchaseService;

namespace TicketingApp.Api.Controllers
{
    [ApiController]
    [Route("V1/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseApiModel p, CancellationToken c)
        {
            try
            {
                var result = await _purchaseService.Create(p.ToDao(), c);
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
        public async Task<IActionResult> Read([FromRoute] int id, CancellationToken c, [FromQuery] bool isIncludeTicket = false)
        {
            try
            {
                var result = await _purchaseService.Read(id, c, isIncludeTicket);
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
        public async Task<IActionResult> List([FromQuery] PurchaseListParamApiModel p, CancellationToken c)
        {
            try
            {
                var result = await _purchaseService.List(p, c);
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
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PurchaseApiModel p, CancellationToken c)
        {
            try
            {
                var result = await _purchaseService.Update(p.ToDao(id), c);
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
                await _purchaseService.Delete(id, c);
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
