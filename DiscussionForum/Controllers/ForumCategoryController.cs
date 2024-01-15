using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    public class ForumCategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
