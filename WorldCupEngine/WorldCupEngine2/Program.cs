using System;
using WorldCupEngine;


namespace WorldCupEngine2
{
    class Program
    {
        static void Main(string[] args)
        {
            WorldCupEngine.ContestentPool cp = new ContestentPool(@"P:\Geo\ages.xlsx", "Stars");
            Console.WriteLine("Hello World!");
            Tournament t = new Tournament(cp, 6, false);
            foreach (Contestent c in t.Contestents)
            {
                System.Diagnostics.Debug.WriteLine(c.Name);
            }


            while (t.CurrentMatch != null)
            {
                Match m = t.CurrentMatch;
                PlayMatch(m, Alphatetic);
                t.NextMatch();
            }
        }

     

        static void PlayMatch(Match m, Func<Match, bool> rules)
        {
            m.result = rules(m) ? Match.Result.firstw : Match.Result.secondw;
        }

        static bool Alphatetic(Match m)
        {
            return String.Compare(m.Item1.Name, m.Item2.Name) < 0;
        }
    }
}
