using DiscussionForum.Models.APIModels;

namespace DiscussionForum.Services
{
    public interface IThreadVoteService
    {
        Task<ThreadVoteDto> CreateThreadVote(ThreadVoteDto threadVoteDto);
    }
}
