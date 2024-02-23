using DiscussionForum.Authorization;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAngularDev")]
    public class EmailController : ControllerBase
    {
        private readonly IMailjetService _mailjetService;

        public EmailController(IMailjetService mailjetService)
        {
            _mailjetService = mailjetService;
        }

        [CustomAuth("User")]
        [HttpPost]
        public async Task<IActionResult> SendEmailAsync([FromBody] EmailModel emailModel)
        {
            try
            {
                await _mailjetService.SendEmailAsync(emailModel.ToEmail, emailModel.Subject, emailModel.Body);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send email: {ex.Message}");
            }
        }
    }

    public class EmailModel
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}

