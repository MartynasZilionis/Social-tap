using SocialTapServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialTapServer.Controllers
{
    public class UserDataController : ApiController
    {
        // GET: api/UserData
        [Route("api/Role/{authToken}")]
        public async Task<UserData> Get(string authToken)
        {
            var fba = new FBAuthorization(Role.Admin, Role.Anonymous, Role.User);
            var usr = await fba.ValidateUser(authToken);
            if (usr.IsBanned) return new UserData(Role.Anonymous);
            if (usr.IsAdmin) return new UserData(Role.Admin);
            return new UserData(Role.User);
        }

        // GET: api/UserData/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserData
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserData/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserData/5
        public void Delete(int id)
        {
        }
    }
}
