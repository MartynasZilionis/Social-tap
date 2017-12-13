using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialTapServer.Models
{
    public class User
    {
        [Key]
        string Id { get; set; }

        bool IsBanned { get; set; }

        bool IsAdmin { get; set; }

        string Name { get; set; }

        public User() : this("", "") { }

        public User(string id, string name)
        {
            Id = id;
            Name = name;
            IsBanned = false;
            IsAdmin = false;
        }
    }
}