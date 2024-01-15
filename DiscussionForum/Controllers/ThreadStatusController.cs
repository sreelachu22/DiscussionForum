using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    public class ThreadStatusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
