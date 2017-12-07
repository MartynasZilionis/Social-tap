using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace social_tapX.RestModels
{
    /// <summary>
    /// A one-time rating about a <see cref="Bar"/>, usually received from a client.
    /// </summary>
    public class Rating
    {
        /// <summary>
        /// How much of the beer mug is filled? 0 if empty, 100 if full, in-between if neither empty nor full.
        /// </summary>
        public float FillPercentage { get; set; }
        
        /// <summary>
        /// The total size of the beer mug in milliliters.
        /// </summary>
        public float MugSize { get; set; }

        /// <summary>
        /// The price of a beer mug in the given bar (in Euros).
        /// </summary>
        public float MugPrice { get; set; }

        /// <summary>
        /// Date when the rating was uploaded.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Default constructor. Fills the object with default values.
        /// </summary>
        public Rating() : this(0, 1000, 0) { }

        public Rating(float fill, float size, float price)
        {
            FillPercentage = fill;
            MugSize = size;
            MugPrice = price;
        }
    }
}
