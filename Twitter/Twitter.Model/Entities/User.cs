using System;
using System.Collections.Generic;
using Twitter.Core.Entity;

namespace Twitter.Model.Entities
{
    public class User : CoreEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public DateTime? BirthDate { get; set; } 
        public DateTime? LastLogin { get; set; }
        public DateTime? LastIPAddress { get; set; }

        public virtual List<Tweet> Tweets { get; set; }
    }
}
