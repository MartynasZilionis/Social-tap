using System;
using System.Collections.Generic;
using System.Text;

namespace social_tapX
{
    public class BarAndCommentsAndRating
    {
        public string BarName { get; private set;}
        public string Comment { get; private set; }
        public int Rating { get; private set; }

        public BarAndCommentsAndRating(string name, string comment, int rating)
        {
            BarName = name;
            Comment = comment;
            Rating = rating;
        }
    }
}
