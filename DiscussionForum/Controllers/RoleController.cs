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

        /* get all roles*/
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetRoles: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /* get single role*/
        [HttpGet("{RoleID}")]
        public async Task<IActionResult> GetRoleByID(int RoleID)
        {
            try
            {
                var role = await _roleService.GetRoleByID(RoleID);
                if (role == null)
                {
                    return NotFound();
                }
                return Ok(role);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetRoleByID: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, "Internal Server Error");
            }
        }


    }
}
