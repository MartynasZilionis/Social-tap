using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Device.Location;
using System.ComponentModel.DataAnnotations;

namespace SocialTapServer.Models
{
    /// <summary>
    /// Information about a bar
    /// </summary>
    public class Bar
    {
        /// <summary>
        /// Unique ID of the bar.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the bar.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Geographic location of the bar.
        /// </summary>
        public GeoCoordinate Location { get; set; }

        /// <summary>
        /// The average mug fill percentage in this bar.
        /// </summary>
        public float AverageFill
        {
            get
            {
                if (Ratings.Count == 0)
                    return 0;
                return (from r in Ratings select r.FillPercentage).Average();
            }
        }

        /// <summary>
        /// The average beer price (EUR/l).
        /// </summary>
        public float AveragePrice
        {
            get
            {
                if (Ratings.Count == 0)
                    return 0;
                return (from r in Ratings select ((r.MugPrice / r.MugSize) * 1000)).Average();
            }
        }

        /// <summary>
        /// Comments uploaded for this bar.
        /// </summary>
        [JsonIgnore]
        public List<Comment> Comments { get; set; }

        /// <summary>
        /// Ratings uploaded for this bar.
        /// </summary>
        [JsonIgnore]
        public List<Rating> Ratings { get; set; }

        public int CommentsCount {
            get
            {
                return Comments.Count;
            }
        }

        public int RatingsCount
        {
            get
            {
                return Ratings.Count;
            }
        }

        /// <summary>
        /// Default constructor. Fills the object with default values.
        /// </summary>
        public Bar() : this(Guid.NewGuid(), "Dummy Bar", new GeoCoordinate(0, 0), new List<Comment>(), new List<Rating>()) { }

        //[JsonIgnore]
        //private List<Comment> comments = new List<Comment>();
        //[JsonIgnore]
        //private List<Rating> ratings = new List<Rating>();
        [JsonIgnore]
        private object commentsLock = new object();
        [JsonIgnore]
        private object ratingsLock = new object();

        public Bar(Guid id, string name, GeoCoordinate location, IEnumerable<Comment> comments, IEnumerable<Rating> ratings)
        {
            Ratings = new List<Rating>(ratings);
            Comments = new List<Comment>(comments);
            Id = id;
            Name = name;
            Location = location;
        }
    }
}