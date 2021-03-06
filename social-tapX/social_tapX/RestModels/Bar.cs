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
        public float AverageFill{ get; set; }

        /// <summary>
        /// The average beer price (EUR/l).
        /// </summary>
        public float AveragePrice { get; set; }

        /// <summary>
        /// The average star rating in this bar's comments.
        /// </summary>
        public float AverageStars { get; set; }

        /// <summary>
        /// Amount of comments uploaded for this bar.
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// Amount of ratings uploaded for this bar.
        /// </summary>
        public int RatingsCount { get; set; }

        /// <summary>
        /// Default constructor. Fills the object with default values.
        /// </summary>
        public Bar() : this("Dummy Bar", new Coordinate(0,0)) { }

        public Bar(string name, Coordinate coord)
        {
            Name = name;
            Location = coord;
        }
    }
}