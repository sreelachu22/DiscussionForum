using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Services;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly AppDbContext _context;

        public BadgeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BadgeDto> GetBadgesByCommunityId(int communityID)
        {
            var badge1 = await _context.Badges.FirstOrDefaultAsync(p => p.CommunityID == communityID);
            var badge = await _context.Badges.FirstOrDefaultAsync(p => p.CommunityID == communityID);

            if (badge == null)
            {
                throw new Exception("No badges found for the specified community.");
            }

            return new BadgeDto
            {
                // Map properties from the retrieved Badge entity to BadgeDto
                GoldMinScore = badge.GoldMinScore,
                SilverMinScore = badge.SilverMinScore,
                BronzeMinScore = badge.BronzeMinScore,
            };
        }


        public async Task<BadgeDto> UpdateBadges(int communityID, BadgeDto badgeDto)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(p => p.CommunityID == communityID);

            if (badge == null)
            {
                return null;
            }

            badge.GoldMinScore = badgeDto.GoldMinScore;
            badge.SilverMinScore = badgeDto.SilverMinScore;
            badge.BronzeMinScore = badgeDto.BronzeMinScore;

            await _context.SaveChangesAsync();

            return badgeDto;
        }
    }
}
