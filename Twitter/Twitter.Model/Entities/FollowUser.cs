using System;
using Twitter.Core.Entity;

namespace Twitter.Model.Entities
{
    public class FollowUser:CoreEntity
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
    }
}
