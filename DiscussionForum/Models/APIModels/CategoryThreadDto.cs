namespace DiscussionForum.Models.APIModels
{
    public class CategoryThreadDto
    {

        public CategoryThreadDto(string title,string content,List<string> tagnames)
        {
            this.Title = title;
            this.Content = content;
            this.TagNames = tagnames;

        }
        public CategoryThreadDto()
        {

        }

        public long ThreadID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid CreatedBy { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string ModifiedByUser { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ThreadStatusName { get; set; }
        public Boolean IsAnswered { get; set; }
        public int UpVoteCount { get; set; }
        public int DownVoteCount { get; set; }

        public int ReplyCount { get; set; }

        public List<String> TagNames { get; set; }

        //public int RepliesCount { get; set; }
    }
}
