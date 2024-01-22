using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAngularDev")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles() {
            var roles = await _roleService.GetAllRoles();
            return Ok(roles);
        }

        [HttpGet("{RoleID}")]
        public async Task<IActionResult> GetRoleByID(int RoleID)
        {
            var role= await _roleService.GetRoleByID(RoleID);
            if (role == null) { 
                return NotFound();
            }
            return Ok(role);
        }

    }
}
