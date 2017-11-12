using SocialTapServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SocialTapServer.Database;
using System.Device.Location;

namespace SocialTapServer.Controllers
{
    public class BarController : ApiController
    {
        // GET: api/GetBars/15.222;16.333/3 (latitude;longitude)
        [Route("api/GetBars/{location}/{number}")]
        public IHttpActionResult Get(string location, int number)
        {
            var parts = location.Split(';');
            try
            {
                return Ok(DatabaseManager.Instance.GetBars(new GeoCoordinate(float.Parse(parts[0]), float.Parse(parts[1])), number));
            }
            catch (FormatException)
            {
                return BadRequest("Invalid coordinates.");
            }
        }

        // GET: api/Bar (WIP)
        public IHttpActionResult Get()
        {
            IEnumerable<Bar> answer = new List<Bar>();
            try
            {
                answer = DatabaseManager.Instance.GetBars(new GeoCoordinate(0, 0), 5);
            }
            catch (Exception)
            {
                InternalServerError();
            }
            return Ok(answer);
        }

        // GET: api/Bar/00000000-0000-0000-0000-000000000000 (WIP)
        public IHttpActionResult Get(Guid id)
        {
            try
            {
                return Ok(DatabaseManager.Instance.GetBar(id));
            }
            catch (KeyNotFoundException)
            {
                return InternalServerError(new InvalidIdException(String.Format("Bar with id {0} not found.", id)));
            }
        }

        // POST: api/Bar
        public void Post([FromBody]Bar value)
        {
            //TODO: retard-proof everything.
            DatabaseManager.Instance.AddBar(value);
        }
    }
}
