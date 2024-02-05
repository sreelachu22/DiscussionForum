using DiscussionForum.Models.APIModels;

namespace DiscussionForum.Services
{
    public interface IReplyVoteService
    {
        Task<int> CreateReplyVote(ReplyVoteDto replyVoteDto);
    }
}
