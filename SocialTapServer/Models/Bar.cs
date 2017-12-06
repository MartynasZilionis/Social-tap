using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Coordinate Location { get; set; }

        /// <summary>
        /// The average mug fill percentage in this bar.
        /// </summary>
        [NotMapped]
        public float AverageFill { get; set; }

        /// <summary>
        /// The average beer price (EUR/l).
        /// </summary>
        [NotMapped]
        public float AveragePrice { get; set; }

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

        [NotMapped]
        public int CommentsCount { get; set; }

        [NotMapped]
        public int RatingsCount { get; set; }

        /// <summary>
        /// Default constructor. Fills the object with default values.
        /// </summary>
        public Bar() : this(Guid.NewGuid(), "Dummy Bar", new Coordinate(0, 0), 0, 0, 0f, 0f) { }

        public Bar(Guid id, string name, Coordinate location, int comments, int ratings, float price, float fill)
        {
            Id = id;
            Name = name;
            Location = location;
            Comments = new List<Comment>();
            Ratings = new List<Rating>();
            CommentsCount = comments;
            RatingsCount = ratings;
            AveragePrice = price;
            AverageFill = fill;
        }
    }
}