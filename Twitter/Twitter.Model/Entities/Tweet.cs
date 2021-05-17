using System;
using Twitter.Core.Entity;

namespace Twitter.Model.Entities
{
    public class Tweet : CoreEntity
    {
        public string TweetDetail { get; set; }
        public string Tags { get; set; }
        public string ImagePath { get; set; }
        public int LikeCount { get; set; }
        public int RetweetCount { get; set; }

        public Guid UserID { get; set; }
        public virtual User User { get; set; }
    }
}
