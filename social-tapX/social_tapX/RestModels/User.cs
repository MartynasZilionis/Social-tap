using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace social_tapX.RestModels
{

    public class User
    {
        /// <summary>
        ///  User facebook Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User facebook Name
        /// </summary>
        public string Name { get; set; }

        public bool IsVerified { get; set; }
        public User() : this("123", "DummyName", false) { }

        public User(string id, string name, bool isVerified)
        {
            Id = id;
            Name = name;
            IsVerified = isVerified;
        }
    }
}
