namespace DiscussionForum.Services
{
    public interface IMailjetService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
