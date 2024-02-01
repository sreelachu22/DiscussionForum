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
<<<<<<< HEAD
            var roles = await _communityservice.GetAllCommunities();
            return Ok(roles);
        } 
=======
            var communities = await _communityservice.GetAllCommunitiesAsync();
            return Ok(communities);
        }
>>>>>>> 470915a7f6db5fa3ad84f0e22d895e8c44ab97a0

    }
}
