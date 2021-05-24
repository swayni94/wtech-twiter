using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Twitter.Core.Map;
using Twitter.Model.Entities;

namespace Twitter.Model.Maps
{
    public class FollowUserMap: CoreMap<FollowUser>
    {
        public override void Configure(EntityTypeBuilder<FollowUser> builder)
        {
            builder.ToTable("FollowUser");
            base.Configure(builder);
        }
    }
}
