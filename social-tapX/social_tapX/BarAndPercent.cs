using System;
using System.Collections.Generic;
using System.Text;

namespace social_tapX
{
    public class BarAndPercent
    {
        public string BarName { get; private set; }
        public int Percent { get; private set; }

        public BarAndPercent(string name, int percent)
        {
            BarName = name;
            Percent = percent;
        }
    }
}
