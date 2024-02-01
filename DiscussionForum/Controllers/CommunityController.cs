using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAngularDev")]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityService _communityservice;

        public CommunityController(ICommunityService communityService)
        {
            _communityservice = communityService;
        }

        //Get all communities
        [HttpGet]
        public async Task<IActionResult> GetCommunities()
        {
            var roles = await _communityservice.GetAllCommunities();
            return Ok(roles);
        } 

    }
}
