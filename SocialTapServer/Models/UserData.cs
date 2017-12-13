using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialTapServer.Models
{
    public class UserData
    {
        public Role Role { get; set; }

        public UserData()
        {
            Role = Role.Anonymous;
        }

        public UserData(Role role)
        {
            Role = role;
        }
    }
}