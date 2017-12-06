using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialTapServer.Models
{
    /// <summary>
    /// Comment about a bar. Usually uploaded alongside a <see cref="Rating"/>.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Unique ID of the comment.
        /// </summary>
        [Key]
        [JsonIgnore]
        public Guid Id { get; set; }
        /// <summary>
        /// Name of whoever wrote the comment.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The comment content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// How many stars the user has given for the bar.
        /// </summary>
        public int Stars { get; set; }

        /// <summary>
        /// Date when the comment was written.
        /// </summary>
        public DateTime Date { get; set; }

        [JsonIgnore]
        public Bar Bar { get; set; }

        /// <summary>
        /// Default constructor. Fills the object with default values.
        /// </summary>
        public Comment()
        {
            Id = Guid.NewGuid();
            Author = "Nobody";
            Content = "Dummy comment";
            Date = DateTime.UtcNow;
            Stars = 0;
        }
    }
}
