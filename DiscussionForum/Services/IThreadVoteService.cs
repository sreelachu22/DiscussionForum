using DiscussionForum.Models.APIModels;

namespace DiscussionForum.Services
{
    public interface IThreadVoteService
    {
        Task CreateThreadVote(ThreadVoteDto threadVoteDto);
    }
}
