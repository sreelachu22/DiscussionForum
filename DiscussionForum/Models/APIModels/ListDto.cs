namespace DiscussionForum.Models.APIModels
{
    public class ListDto<T>
    {
        public T Id { get; set; }

        public string Name { get; set; }
    }
}
