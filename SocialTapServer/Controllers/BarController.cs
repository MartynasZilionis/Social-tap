using SocialTapServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SocialTapServer.Database;
using System.Device.Location;
using System.Threading.Tasks;

namespace SocialTapServer.Controllers
{
    public class BarController : ApiController
    {
        // GET: api/GetBars/15.222222;16.333333/3 (latitude;longitude)
        [Route("api/GetBars/{location}/{number}")]
        public async Task<IHttpActionResult> Get(string location, int number)
        {
            var parts = location.Split(';');
            try
            {
                return Ok(await DatabaseManager.Instance.GetBars(new Coordinate(float.Parse(parts[0]), float.Parse(parts[1])), number));
            }
            catch (FormatException)
            {
                return BadRequest("Invalid coordinates.");
            }
        }

        // GET: api/GetTopBars/0/10
        [Route("api/GetTopBars/{index}/{count}")]
        public async Task<IHttpActionResult> GetTop(int index, int count)
        {
            return Ok(await DatabaseManager.Instance.GetTopBars(index, count));
        }

        // GET: api/Bar (WIP)
        public async Task<IHttpActionResult> Get()
        {
            IEnumerable<Bar> answer = new List<Bar>();
            try
            {
                answer = await DatabaseManager.Instance.GetBars(new Coordinate(0, 0), 5);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(answer);
        }

        // GET: api/Bar/00000000-0000-0000-0000-000000000000 (WIP)
        public async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                return Ok(await DatabaseManager.Instance.GetBar(id));
            }
            catch (KeyNotFoundException)
            {
                return InternalServerError(new InvalidIdException(String.Format("Bar with id {0} not found.", id)));
            }
        }

        // POST: api/Bar
        public async Task Post([FromBody]Bar value)
        {
            //TODO: retard-proof everything.
            await DatabaseManager.Instance.AddBar(value);
        }
    }
}
