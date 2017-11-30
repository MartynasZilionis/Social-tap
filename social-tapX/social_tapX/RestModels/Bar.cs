﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace social_tapX.RestModels
{
    /// <summary>
    /// Information about a bar
    /// </summary>
    public class Bar
    {
        /// <summary>
        /// Unique ID of the bar.
        /// </summary>
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
        public float Score
        {
            get
            {
                if (ratings.Count == 0)
                    return 0;
                return (from r in ratings select r.FillPercentage).Average();
            }
        }

        /// <summary>
        /// The average beer price (EUR/l).
        /// </summary>
        public float AveragePrice
        {
            get
            {
                if (ratings.Count == 0)
                    return 0;
                return (from r in ratings select ((r.MugPrice / r.MugSize) * 1000)).Average();
            }
        }

        /// <summary>
        /// Amount of comments uploaded for this bar.
        /// </summary>
        public int Comments
        {
            get
            {
                return comments.Count;
            }
        }

        /// <summary>
        /// Amount of ratings uploaded for this bar.
        /// </summary>
        public int Ratings
        {
            get
            {
                return ratings.Count;
            }
        }

        /// <summary>
        /// Default constructor. Fills the object with default values.
        /// </summary>
        public Bar()
        {
            Id = Guid.NewGuid();
            Name = "Dummy Bar";
        }

        [JsonIgnore]
        private List<Comment> comments = new List<Comment>();
        [JsonIgnore]
        private List<Rating> ratings = new List<Rating>();
        [JsonIgnore]
        private object commentsLock = new object();
        [JsonIgnore]
        private object ratingsLock = new object();

        public Bar(Guid id, string name, IEnumerable<Comment> comments, IEnumerable<Rating> ratings)
        {
            Id = id;
            this.comments.AddRange(comments);
            this.ratings.AddRange(ratings);
            Name = name;
        }

        public IEnumerable<Rating> GetRatings(int index, int count)
        {
            return ratings.GetRange(index, count);
        }
        
        public IEnumerable<Comment> GetComments(int index, int count)
        {
            return comments.GetRange(index, count);
        }

        public void AddRating(Rating rating)
        {
            lock(ratingsLock)
            {
                ratings.Add(rating);
            }
        }

        public void AddComment(Comment comment)
        {
            lock (commentsLock)
            {
                comments.Add(comment);
            }
        }
    }
}