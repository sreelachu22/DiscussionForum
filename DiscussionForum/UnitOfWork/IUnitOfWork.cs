using DiscussionForum.Repositories;

namespace DiscussionForum.UnitOfWork
{
    public interface IUnitOfWork
    {
        IDesignationRepository Designations { get; }
        IRoleRepository Role { get; }
        ICommunityCategoryRepository CommunityCategory { get; }
        ICommunityStatusRepository CommunityStatus { get; }
        IThreadStatusRepository ThreadStatus { get; }
        ICommunityRepository Community { get; }
        INoticeRepository Notice { get; }
        IUserRepository User { get; }
        IThreadRepository Threads { get; }
        IReplyRepository Reply { get; }
        IThreadVoteRepository ThreadVote { get; }
        ITagRepository Tag { get; }
        ISavedPostRepository SavedPost { get; }
        int Complete();

    }

}
