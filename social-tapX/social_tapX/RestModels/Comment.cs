using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace social_tapX.RestModels
{
    /// <summary>
    /// Comment about a bar. Usually uploaded alongside a <see cref="Rating"/>.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Name of whoever wrote the comment.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The comment content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Date when the comment was written.
        /// </summary>
        [JsonIgnore]
        public DateTime Date { get; set; }

        /// <summary>
        /// Default constructor. Fills the object with default values.
        /// </summary>
        public Comment() : this("Nobody", "Dummy comment") { }

        public Comment(string author, string content)
        {
            Author = author;
            Content = content;
        }

        public override string ToString()
        {
            return Content;
        }

    }
}
