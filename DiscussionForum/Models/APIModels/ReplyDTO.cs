﻿using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Models.APIModels
{
    //DTO for generating all the replies of a thread in a nested reply manner.
    //Each reply will have its child replies stored in NestedReplies list.
    public class ReplyDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReplyID { get; set; }

        public long ThreadID { get; set; }

        [ForeignKey("ThreadID")]


        [JsonIgnore]
        public virtual Threads Thread { get; set; }

        public long? ParentReplyID { get; set; }

        [ForeignKey("ParentReplyID")]
        [JsonIgnore]
        public virtual ReplyDTO ParentReply { get; set; }

        public string Content { get; set; }
        public List<ReplyDTO> NestedReplies { get; set; }

    }
}
