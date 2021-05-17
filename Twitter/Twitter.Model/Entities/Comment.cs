using System;
using Twitter.Core.Entity;

namespace Twitter.Model.Entities
{
    public class Comment : CoreEntity
    {
        public string CommentText { get; set; }

        public int UserID { get; set; }
        public virtual User User { get; set; }


        public int TweetID { get; set; }
        public virtual Tweet Tweet { get; set; }
    }
}
