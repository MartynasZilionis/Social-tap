using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SocialTapServer.Models;
using SocialTapServer.Database;

namespace SocialTapServer.Controllers
{
    public class RatingController : ApiController
    {
        // GET: api/Rating/00000000-0000-0000-0000-000000000000
        public IHttpActionResult Get(Guid id)
        {
            var bar = DatabaseManager.Instance.GetBar(id);
            
            return Ok(new Rating(bar.AverageFill, 1000, bar.AveragePrice, DateTime.MinValue));
        }

        // GET: api/Rating/00000000-0000-0000-0000-000000000000/0/10
        [Route("api/Rating/{id}/{index}/{count}")]
        public IHttpActionResult Get(Guid id, int index, int count)
        {
            return Ok(DatabaseManager.Instance.GetRatings(id, index, count));
        }

        // POST: api/Rating/00000000-0000-0000-0000-000000000000
        public async void Post([FromBody]Rating value, Guid id)
        {
            await DatabaseManager.Instance.AddRating(id, value);
        }
    }
}
