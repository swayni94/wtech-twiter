using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Twitter.Core.Map;
using Twitter.Model.Entities;

namespace Twitter.Model.Maps
{
    public class UserMap : CoreMap<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Employees");
            builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.Property(x => x.ImagePath).HasMaxLength(250).IsRequired(false);
            builder.Property(x => x.EmailAddress).HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.Password).HasMaxLength(1000).IsRequired(true);
            builder.Property(x => x.LastLogin).IsRequired(false);
            builder.Property(x => x.LastIPAddress).HasMaxLength(24).IsRequired(false);
            base.Configure(builder);
        }
    }
}
