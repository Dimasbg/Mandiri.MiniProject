using Mandiri.MiniProject.Utilities.Base;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserAuthApp.Api.Models.UserRoleModel;
using UserAuthApp.Data.Dao;
using UserAuthApp.Domain.RoleService;
using UserAuthApp.Domain.UserRoleService;

namespace UserAuthApp.Api.Controllers
{
    [ApiController]
    [Route("V1/[controller]")]
    public class UserRoleCtroller : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleCtroller(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpPost("Assign")]
        public async Task<IActionResult> AssignUserToRole([FromBody] UserRoleModelApiModel p, CancellationToken c)
        {
            try
            {
                await _userRoleService.AssignUserToRole(p.UserId, p.RoleId, c);
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

        [HttpDelete("Assign")]
        public async Task<IActionResult> RevokeUserFromRole([FromBody] UserRoleModelApiModel p, CancellationToken c)
        {
            try
            {
                await _userRoleService.RevokeUserFromRole(p.UserId, p.RoleId, c);
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

