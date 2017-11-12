using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialTapServer.Models
{
    /// <summary>
    /// A detailed profile of a <see cref="Bar"/>. Includes all <see cref="Comment">Comments</see>, average score and average beer price.
    /// </summary>
    /// <remarks>
    /// Probably not gonna be used.
    /// </remarks>
    public class BarProfile
    {
        /// <summary>
        /// Id of the bar. Should not be sent over network.
        /// </summary>
        [JsonIgnore]
        public Guid BarId { get; set; }

        /// <summary>
        /// Comments about the bar, uploaded by users.
        /// </summary>
        public Comment[] Comments { get; set; }

        /// <summary>
        /// Average score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Average beer price per liter (EUR).
        /// </summary>
        public float Price { get; set; }

        /// <summary>
        /// Default constructor. Fills the object with default values.
        /// </summary>
        public BarProfile()
        {
            BarId = new Guid();
            Comments = new Comment[] { new Comment(), new Comment() };
            Score = 5;
            Price = 5;
        }
    }
}
