using System;
using System.Collections.Generic;
using System.Text;

namespace social_tapX.RestModels
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
