namespace DiscussionForum.Services
{
    using DiscussionForum.Models.EntityModels;
    public interface IForumCategoryService
    {
        Task<IEnumerable<ForumCategory>> GetForumCategoriesAsync();
        Task<ForumCategory> GetForumCategoryByIdAsync(long id);
        Task<ForumCategory> CreateForumCategoryAsync(string forumCategoryName);
        Task<ForumCategory> UpdateForumCategoryAsync(long id, ForumCategory forumCategoryDto);
        Task<ForumCategory> DeleteForumCategoryAsync(long id);
    }
}
