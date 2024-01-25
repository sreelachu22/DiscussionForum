/*using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services
{
    public class UserRoleMappingService : IUserRoleMappingService
    {
        private readonly AppDbContext _context;

        public CommunityCategoryMappingService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CommunityCategoryMapping> SetUserRole(Guid userId, long newRoleId)(int communityCategoryMappingID, CommunityCategoryMappingAPI model)
        {
            var entity = await _context.CommunityCategoryMapping
                .FirstOrDefaultAsync(ccm => ccm.CommunityCategoryMappingID == communityCategoryMappingID && !ccm.IsDeleted);

            *//*if (entity == null)
                return false;*//*

            entity.Description = model.Description;
            entity.ModifiedBy = model.ModifiedBy;
            entity.IsDeleted = false;
            entity.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
*/