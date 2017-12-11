using SocialTapServer.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;

namespace SocialTapServer.Controllers
{
    public class FeedbackController : ApiController
    {
        // POST: api/Feedback
        public async Task Post([FromBody]string value)
        {
            await DatabaseManager.Instance.AddFeedback(value);
        }
    }
}
