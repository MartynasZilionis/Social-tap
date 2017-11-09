namespace social_tapX
{
    public class Rated_Bar
    {
        public string Name { get; private set; }
        public string Percent { get; private set; }
        public string Rating { get; private set; }

        public Rated_Bar(string name, double percent, double rating)
        {
            Name = name;
            Percent = "" + percent;
            Rating = "" + rating;
        }
    }
}