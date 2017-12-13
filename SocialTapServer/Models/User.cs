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
        public string Id { get; set; }

        public bool IsBanned { get; set; }

        public bool IsAdmin { get; set; }

        public string Name { get; set; }

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