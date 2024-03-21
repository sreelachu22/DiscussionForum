namespace DiscussionForum.ExceptionFilter
{
    public class CustomException : Exception
    {
        public int statusCode;
        public string? message;
        public CustomException(int statusCode, string? message) : base(message)
        {
            this.statusCode = statusCode;
            this.message = message;
        }
    }
}
