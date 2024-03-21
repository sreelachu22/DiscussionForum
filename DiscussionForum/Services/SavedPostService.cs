using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services
{
    public class SavedPostService : ISavedPostService
    {
        private readonly AppDbContext _context;

        public SavedPostService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SavePost(SavedPostDTO savedPostDto)
        {
            try
            {
                var savedPost = new SavedPosts
                {
                    UserID = savedPostDto.UserID,
                    ThreadID = savedPostDto.ThreadID,
                    SavedAt = DateTime.Now
                    // Set any other properties as needed
                };

                _context.SavedPosts.Add(savedPost);
                await _context.SaveChangesAsync();
                return savedPost.SavedPostID; // Return the generated SavedPostID
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while saving post: {ex.Message}", ex);
            }
        }

        public async Task DeleteSavedPost(Guid userId, int threadId)
        {
            var savedPost = await _context.SavedPosts.FirstOrDefaultAsync(sp => sp.UserID == userId && sp.ThreadID == threadId);
            if (savedPost != null)
            {
                _context.SavedPosts.Remove(savedPost);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<List<SavedPosts>> GetSavedPostsByUserId(Guid userId)
        {
            return await _context.SavedPosts.Where(sp => sp.UserID == userId).ToListAsync();
        }
    }
}
