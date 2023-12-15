using Mandiri.MiniProject.Utilities.Base;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserAuthApp.Api.Models.RoleModels;
using UserAuthApp.Api.Models.UserModels;
using UserAuthApp.Data.Dao;
using UserAuthApp.Domain.RoleService;
using UserAuthApp.Domain.UserService;

namespace UserAuthApp.Api.Controllers
{
    [ApiController]
    [Route("V1/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string name, CancellationToken c)
        {
            try
            {
                var result = await _roleService.Create(name, c);
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
        public async Task<IActionResult> Read([FromRoute] int id, [FromQuery] bool isIncludeUser, CancellationToken c)
        {
            try
            {
                var result = await _roleService.Read(id, c, isIncludeUser);
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
        public async Task<IActionResult> List([FromQuery] RoleListParamApiModel p, CancellationToken c)
        {
            try
            {
                var result = await _roleService.List(p, c);
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
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Role p, CancellationToken c)
        {
            try
            {
                p.RoleId = id;
                var result = await _roleService.Update(p, c);
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
                await _roleService.Delete(id, c);
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
