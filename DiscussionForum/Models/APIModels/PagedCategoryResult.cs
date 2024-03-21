namespace DiscussionForum.Models.APIModels
{
    public class PagedCategoryResult
    {
        public IReadOnlyCollection<CommunityCategoryMappingAPI> Categories { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
