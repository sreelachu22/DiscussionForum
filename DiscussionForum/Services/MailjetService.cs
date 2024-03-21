using System.Net.Mail;
using System.Net;

namespace DiscussionForum.Services
{
    public class MailjetService : IMailjetService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            string apiKey = "d0d7a435f149f5641cc061dc7756b30f";
            string apiSecret = "1a3a0ea1813d49997cdb7c29e67d601e";

            using (var client = new SmtpClient("in-v3.mailjet.com"))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(apiKey, apiSecret);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("merinjose127@gmail.com", "Admin"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(new MailAddress(toEmail));

                try
                {
                    await client.SendMailAsync(mailMessage);
                    Console.WriteLine("Email sent successfully!");
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }
    }
}
