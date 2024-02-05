using DiscussionForum.Models.APIModels;

namespace DiscussionForum.Services
{
    public interface IThreadVoteService
    {
        Task<int> CreateThreadVote(ThreadVoteDto threadVoteDto);
    }
}
