using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    public class DesignationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
