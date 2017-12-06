using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SocialTapServer.Models;
using SocialTapServer.Database;
using System.Threading.Tasks;

namespace SocialTapServer.Controllers
{
    public class RatingController : ApiController
    {
        // GET: api/Rating/00000000-0000-0000-0000-000000000000
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var bar = await DatabaseManager.Instance.GetBar(id);
            
            return Ok(new Rating(bar.AverageFill, 1000, bar.AveragePrice, DateTime.MinValue));
        }

        // GET: api/Rating/00000000-0000-0000-0000-000000000000/0/10
        [Route("api/Rating/{id}/{index}/{count}")]
        public async Task<IHttpActionResult> Get(Guid id, int index, int count)
        {
            return Ok(await DatabaseManager.Instance.GetRatings(id, index, count));
        }

        // POST: api/Rating/00000000-0000-0000-0000-000000000000
        public async Task Post([FromBody]Rating value, Guid id)
        {
            await DatabaseManager.Instance.AddRating(id, value);
        }
    }
}
