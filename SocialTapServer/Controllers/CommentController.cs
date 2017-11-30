using SocialTapServer.Database;
using SocialTapServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SocialTapServer.Controllers
{
    public class CommentController : ApiController
    {
        // GET: api/Comment/00000000-0000-0000-0000-000000000000/0/10
        [Route("api/Comment/{id}/{index}/{count}")]
        public IHttpActionResult Get(Guid id, int index, int count)
        {
            return Ok(DatabaseManager.Instance.GetComments(id, index, count));
        }

        // POST: api/Comment/00000000-0000-0000-0000-000000000000
        public async void Post([FromBody]Comment value, Guid id)
        {
            await DatabaseManager.Instance.AddComment(id, value);
        }
    }
}
