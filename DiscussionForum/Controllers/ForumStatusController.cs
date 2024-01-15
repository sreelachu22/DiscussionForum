using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    public class ForumStatusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
