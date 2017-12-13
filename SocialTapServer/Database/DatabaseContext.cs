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
#if DEBUG
        public DatabaseContext() : base("LocalDatabaseContext") { }
#else
        public DatabaseContext() : base("DatabaseContext") { }
#endif
        public DbSet<Bar> Bars { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<User> Users { get; set; }
    }
}