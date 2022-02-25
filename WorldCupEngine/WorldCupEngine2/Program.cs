﻿using System;
using WorldCupEngine;
using System.IO;
using System.Collections;


namespace WorldCupEngine2
{
    class Program
    {
        static void Main(string[] args)
        {
            WorldCupEngine.ContestentPool cp = new ContestentPool(@"P:\Geo\WorldCup.xlsx", "Sheet1");
            Console.WriteLine("Hello World!");
          
            for (int i = 0; i < 10; i++)
            {
                Tournament t = new Tournament(cp, 8, false);
                using (StreamWriter sw = new StreamWriter("pwc.txt"))
                {
                    int cround = 0;
                    while (t.CurrentMatch != null)
                    {
                        Round r = t.CurrentRound;
                        if (r.RoundNumber != cround)
                        {
                            sw.WriteLine($"Round {r.RoundNumber}\n ");
                            foreach (Contestent c in r.GetContestents())
                            {
                                sw.WriteLine(c.Name);
                            }
                            cround = r.RoundNumber;
                        }

                        Match m = t.CurrentMatch;
                        PlayMatch(m, Random);
                        sw.WriteLine($" {m.Item1.Name} vs {m.Item2.Name} won by {m.Winner().Name}");
                        t.NextMatch();
                    }
                    sw.WriteLine($"They think its all over the winner was {t.Winner().Name}");
                }
            }
            cp.Export(@"P:\Geo\WorldCup.xlsx", "Sheet1");

         
        }

     

        static void PlayMatch(Match m, Func<Match, bool> rules)
        {
            m.result = rules(m) ? Match.Result.firstw : Match.Result.secondw;
        }

        static bool Alphatetic(Match m)
        {
            return String.Compare(m.Item1.Name, m.Item2.Name) < 0;
        }

        static bool Random(Match m)
        {
            Random r = new Random();
            return r.Next(1, 3) == 1;
        }
    }
}
