using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Twitter.Core.Entity;
using Twitter.Model.Entities;
using Twitter.Model.Maps;

namespace Twitter.Model.Context
{
    public class TwitterContext : DbContext
    {
        public TwitterContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CommentMap());
            modelBuilder.ApplyConfiguration(new TweetMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges()
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified
                || x.State == EntityState.Added).ToList();

            string computerName = Environment.MachineName;
            string ipAddress = "127.0.0.1";
            DateTime date = DateTime.Now;

            foreach (var item in modifiedEntities)
            {
                CoreEntity entity = item.Entity as CoreEntity;
                if (item != null)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            entity.CreatedComputerName = computerName;
                            entity.CreatedIP = ipAddress;
                            entity.CreatedDate = date;
                            break;

                        case EntityState.Modified:
                            entity.ModifiedComputerName = computerName;
                            entity.ModifiedIP = ipAddress;
                            entity.ModifiedDate = date;
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}
