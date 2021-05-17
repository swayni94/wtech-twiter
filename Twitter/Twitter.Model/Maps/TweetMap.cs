using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Twitter.Core.Map;
using Twitter.Model.Entities;

namespace Twitter.Model.Maps
{
    public class TweetMap : CoreMap<Tweet>
    {
        public override void Configure(EntityTypeBuilder<Tweet> builder)
        {
            builder.ToTable("Tweets");
            builder.Property(x => x.TweetDetail).IsRequired(true);
            builder.Property(x => x.Tags).IsRequired(true);
            builder.Property(x => x.ImagePath).IsRequired(false);
            builder.Property(x => x.LikeCount).IsRequired(true);
            builder.Property(x => x.RetweetCount).IsRequired(true);
            base.Configure(builder);
        }
    }
}
