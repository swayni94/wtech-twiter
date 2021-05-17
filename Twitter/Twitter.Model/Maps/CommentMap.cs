using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Twitter.Core.Map;
using Twitter.Model.Entities;

namespace Twitter.Model.Maps
{
    public class CommentMap : CoreMap<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.Property(x => x.CommentText).IsRequired();
            base.Configure(builder);
        }
    }
}
