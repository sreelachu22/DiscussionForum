namespace DiscussionForum.Models.APIModels
{
    public class ErrorDTO
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
    }
}
