using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialTapServer.Models
{
    public class Coordinate
    {
        public Coordinate() : this(0, 0) { }
        public Coordinate(double la, double lo)
        {
            Longitude = lo;
            Latitude = la;
        }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}