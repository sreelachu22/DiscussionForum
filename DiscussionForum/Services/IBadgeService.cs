using DiscussionForum.Models.APIModels;

namespace DiscussionForum.Services
{
    public interface IBadgeService
    {
        Task<BadgeDto> UpdateBadges(int communityID, BadgeDto badgeDto);
        Task<BadgeDto> GetBadgesByCommunityId(int communityId);
    }
}
