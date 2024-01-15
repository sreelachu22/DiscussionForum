using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
