namespace DiscussionForum.Models.APIModels
{
    public class ExternalAuthDto
    {
        //for microsoft authentication
        public string Token { get; set; }

        public string Provider { get; set; }
    }
}
