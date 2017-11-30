using SocialTapServer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SocialTapServer.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Bar> Bars { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}