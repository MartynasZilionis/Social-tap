using System.Collections.Generic;

namespace social_tapX
{
    static public class BarsToList
    {
        static public List<Rated_Bar> ListOfBars = new List<Rated_Bar>();

        //Surisu Pauliaus, cia gauna duomenis apie bara ir sudeda i List, kuri poto galima perduot is esmes betkur

        public static void MakeList()
        {
            for (int i = 0; i < 5; i++)
            {
                ListOfBars.Add(new Rated_Bar("Trys Draugai", 85.45 + i, 9.4 + (double)i / 10));
            }

            for (int i = 5; i < 10; i++)
            {
                ListOfBars.Add(new Rated_Bar("Du Draugai", 75.45 + i, 9.4 - (double)i / 10));
            }

            for (int i = 10; i < 15; i++)
            {
                ListOfBars.Add(new Rated_Bar("Vienas Draugas", 25.45 + i, 5.4 + (double)i / 10));
            }

            for (int i = 15; i < 20; i++)
            {
                ListOfBars.Add(new Rated_Bar("(╯°□°）╯︵ ┻━┻", 5.45 + i, 0.4 + (double)i / 10));
            }
        }

        public static List<Rated_Bar> GetListOfBars() { return ListOfBars; }
    }
}