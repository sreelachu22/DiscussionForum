﻿using DiscussionForum.Models.APIModels;

namespace DiscussionForum.Services
{
    public interface IReplyVoteService
    {        
        Task VoteAsync(ReplyVoteDto voteDto);
    }
}
