using DiscussionForum.Authorization;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [EnableCors("AllowAngularDev")]
    [Route("api/[controller]")]
    public class NoticeController : ControllerBase
    {
        private readonly INoticeService _noticeService;

        public NoticeController(INoticeService NoticeService)
        {
            _noticeService = NoticeService;
        }

        [CustomAuth("User")]
        [HttpGet]
        public async Task<IActionResult> GetNotices()
        {
            var notices = await _noticeService.GetNoticesAsync();
            return Ok(notices);
        }

        [CustomAuth("Admin")]
        [HttpPost] // Call the notice service to create a notice using the provided data from the NoticeDto.
        public async Task<IActionResult> CreateNotice([FromBody] NoticeDto NoticeDto)
        {
            var notice = await _noticeService.CreateNoticeAsync(NoticeDto.CommunityID, NoticeDto.Title, NoticeDto.Content, NoticeDto.ExpiresAt, NoticeDto.CreatedBy);
            return Ok(notice);
        }

        [CustomAuth("Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotice(int id, [FromBody] NoticeDto NoticeDto)
        {
            var result = await _noticeService.UpdateNoticeAsync(id, NoticeDto.CommunityID, NoticeDto.Title, NoticeDto.Content, NoticeDto.ExpiresAt, NoticeDto.ModifiedBy);

            if (result == null)
                return NotFound();

            return Ok(result); // Return an HTTP 200 OK response with the updated notice.
        }


        [CustomAuth("Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotice(int id)
        {
            var result = await _noticeService.DeleteNoticeAsync(id);

            if (result == null)
                return NotFound();

            return NoContent();
        }
    }
}
