using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCupEngine;
using System.IO;


namespace WordCupTests
{
    [DeploymentItem("..\\..\\..\\NewCelebs.xlsx")]


    [TestClass]
    public class WorldCupTests
    {
        [TestMethod]
        public void ContestentPoolNew()
        {
            ContestentPool contestents = new ContestentPool("NewCelebs.xlsx", "Sheet1");
            Assert.AreEqual(106, contestents.Count);
            Assert.IsTrue(contestents.All(x => x.Tornaments == 0));
            Assert.IsTrue(contestents.All(x => x.Points == 0));
            Assert.IsTrue(contestents.All(x => x.TournementWins == 0));
            Assert.IsTrue(contestents.All(x => x.Losses == 0));
            Assert.IsTrue(contestents.All(x => x.Wins == 0));
        }

        [TestMethod]
        public void OneTournament()
        {
            Random random = new Random();
            File.Copy("NewCelebs.xlsx", "CelebsOT.xlsx");
            ContestentPool contestents = new ContestentPool("CelebsOT.xlsx",string.Empty);
            Tournament t = new Tournament(contestents, 6, Tournament.Format.standard);
            Round r = t.CurrentRound;
            while (r != null)
            {
                while (r.AllMatches.Where(x=>x.result == Match.Result.notplayed).Any())
                {
                    Match[] matcharray = r.AllMatches.Where(x => x.result == Match.Result.notplayed).ToArray();
                    Match randmatch = matcharray[random.Next(matcharray.Length)];
                    PlayMatch(randmatch,Alphatetic);
                }
                r = t.NextRound();
            }
            t.Update("CelebsOT.xlsx", String.Empty);
            ContestentPool checkcontestents = new ContestentPool("CelebsOT.xlsx", string.Empty);
            OneTournamentAssertions(checkcontestents, 6);
        }

        [TestMethod]
        public void OneTournamentWithSave()
        {
            Random random = new Random();
            File.Copy("NewCelebs.xlsx", "CelebsOTWS.xlsx");
            ContestentPool contestents = new ContestentPool("CelebsOTWS.xlsx", string.Empty);
            Tournament t = new Tournament(contestents, 6, Tournament.Format.standard);
            Round r = t.CurrentRound;
            int tock = 40;

            while (tock > 0)
            {
                while (tock > 0 && r.AllMatches.Where(x => x.result == Match.Result.notplayed).Any())
                {
                    Match[] matcharray = r.AllMatches.Where(x => x.result == Match.Result.notplayed).ToArray();
                    Match randmatch = matcharray[random.Next(matcharray.Length)];
                    PlayMatch(randmatch, Alphatetic);
                    tock--;
                }
                if (r.AllMatches.Where(x => x.result == Match.Result.notplayed).Any() == false)
                {
                    r = t.NextRound();
                }
            }
            
            t.Save("OTWS.xlsx");
            Tournament t2 = new Tournament(contestents);
            t2.Load("OTWS.xlsx");

            r = t2.CurrentRound;
            while (r != null)
            {
                while (r.AllMatches.Where(x => x.result == Match.Result.notplayed).Any())
                {
                    Match[] matcharray = r.AllMatches.Where(x => x.result == Match.Result.notplayed).ToArray();
                    Match randmatch = matcharray[random.Next(matcharray.Length)];
                    PlayMatch(randmatch, Alphatetic);
                }
                r = t2.NextRound();
            }

            t2.Update("CelebsOTWS.xlsx", String.Empty);
            ContestentPool checkcontestents = new ContestentPool("CelebsOTWS.xlsx", string.Empty);
            OneTournamentAssertions(checkcontestents, 6);

         }

        [TestMethod]
        public void OneTournamentWithSave2Pools()
        {
            Random random = new Random();
            File.Copy("NewCelebs.xlsx", "CelebsOTWS2p.xlsx");
            ContestentPool contestents = new ContestentPool("CelebsOTWS2p.xlsx", string.Empty);
            Tournament t = new Tournament(contestents, 6, Tournament.Format.standard);
            Round r = t.CurrentRound;
            int tock = 40;

            while (tock > 0)
            {
                while (tock > 0 && r.AllMatches.Where(x => x.result == Match.Result.notplayed).Any())
                {
                    Match[] matcharray = r.AllMatches.Where(x => x.result == Match.Result.notplayed).ToArray();
                    Match randmatch = matcharray[random.Next(matcharray.Length)];
                    PlayMatch(randmatch, Alphatetic);
                    tock--;
                }
                if (r.AllMatches.Where(x => x.result == Match.Result.notplayed).Any() == false)
                {
                    r = t.NextRound();
                }
            }

            t.Save("OTWS2p.xlsx");
            contestents = new ContestentPool("CelebsOTWS2p.xlsx", string.Empty);
            Tournament t2 = new Tournament(contestents);
            t2.Load("OTWS2p.xlsx");

            r = t2.CurrentRound;
            while (r != null)
            {
                while (r.AllMatches.Where(x => x.result == Match.Result.notplayed).Any())
                {
                    Match[] matcharray = r.AllMatches.Where(x => x.result == Match.Result.notplayed).ToArray();
                    Match randmatch = matcharray[random.Next(matcharray.Length)];
                    PlayMatch(randmatch, Alphatetic);
                }
                r = t2.NextRound();
            }

            t2.Update("CelebsOTWS2p.xlsx", String.Empty);
            ContestentPool checkcontestents = new ContestentPool("CelebsOTWS2p.xlsx", string.Empty);
            OneTournamentAssertions(checkcontestents, 6);

        }

        [TestMethod]
        public void TwoTournaments()
        {
            Random random = new Random();
            File.Copy("NewCelebs.xlsx", "CelebsTT.xlsx");
            for (int i = 0; i < 2; i++)
            {
                ContestentPool contestents = new ContestentPool("CelebsTT.xlsx", string.Empty);
                Tournament t = new Tournament(contestents, 5, Tournament.Format.standard);
                Round r = t.CurrentRound;
                while (r != null)
                {
                    while (r.AllMatches.Where(x => x.result == Match.Result.notplayed).Any())
                    {
                        Match[] matcharray = r.AllMatches.Where(x => x.result == Match.Result.notplayed).ToArray();
                        Match randmatch = matcharray[random.Next(matcharray.Length)];
                        PlayMatch(randmatch, Alphatetic);
                    }
                    r = t.NextRound();
                }
                t.Update("CelebsTT.xlsx", String.Empty);
            }
            ContestentPool checkcontestents = new ContestentPool("CelebsTT.xlsx", string.Empty);
            Assert.AreEqual(64, checkcontestents.Sum(x => x.Tornaments));
            Assert.AreEqual(2, checkcontestents.Sum(x => x.TournementWins));
            Assert.AreEqual(62,checkcontestents.Sum(x => x.Losses));
            Assert.AreEqual(96, checkcontestents.Sum(x => x.Points));
        }

        [TestMethod]
        public void TwoTournamentsIntersectionTest()
        {
            Random random = new Random();
            File.Copy("NewCelebs.xlsx", "CelebsTT2.xlsx");
         
            ContestentPool contestents = new ContestentPool("CelebsTT2.xlsx", string.Empty);
            Tournament t = new Tournament(contestents, 5, Tournament.Format.standard, 721);
            IEnumerable<Contestent> t1contestents = t.Contestents;
            IEnumerable<Contestent> t2contestents = null;
            IEnumerable<string> luckynames = null;
            Tournament t2 = null;

            Round r = t.CurrentRound;
            while (r != null)
            {
                while (r.AllMatches.Where(x => x.result == Match.Result.notplayed).Any())
                {
                    Match[] matcharray = r.AllMatches.Where(x => x.result == Match.Result.notplayed).ToArray();
                    Match randmatch = matcharray[random.Next(matcharray.Length)];
                    PlayMatch(randmatch, Alphatetic);
                }
                r = t.NextRound();
            }

            Assert.AreEqual("Alex Higgins", t.Winner().Name);
            t.Update("CelebsTT2.xlsx", String.Empty);


            ContestentPool contestents2 = new ContestentPool("CelebsTT2.xlsx", string.Empty);
           
            t2 = new Tournament(contestents2, 5, Tournament.Format.standard, 1432);
            t2contestents = t2.Contestents;
            luckynames = t1contestents.Select(x => x.Name).Intersect(t2contestents.Select(x => x.Name));
            
            

            r = t2.CurrentRound;
            int tock = 21;

            while (tock > 0)
            {
                while (tock > 0 && r.AllMatches.Where(x => x.result == Match.Result.notplayed).Any())
                {
                    Match[] matcharray = r.AllMatches.Where(x => x.result == Match.Result.notplayed).ToArray();
                    Match randmatch = matcharray[random.Next(matcharray.Length)];
                    PlayMatch(randmatch, Alphatetic);
                    tock--;
                }
                if (r.AllMatches.Where(x => x.result == Match.Result.notplayed).Any() == false)
                {
                    r = t2.NextRound();
                }
            }

            t2.Save("OTWS.xlsx");
            Tournament t22 = new Tournament(contestents);
            t22.Load("OTWS.xlsx");

            r = t22.CurrentRound;
            while (r != null)
            {
                while (r.AllMatches.Where(x => x.result == Match.Result.notplayed).Any())
                {
                    Match[] matcharray = r.AllMatches.Where(x => x.result == Match.Result.notplayed).ToArray();
                    Match randmatch = matcharray[random.Next(matcharray.Length)];
                    PlayMatch(randmatch, Alphatetic);
                }
                r = t22.NextRound();
            }
            Assert.AreEqual("Aaron Paul", t22.Winner().Name);
            t22.Update("CelebsTT2.xlsx", String.Empty);

            ContestentPool checkContestents = new ContestentPool("CelebsTT2.xlsx", String.Empty);
            List<Contestent> luckyCheck = new List<Contestent>();
            foreach (string name in luckynames)
            {
                luckyCheck.Add(checkContestents[name]);
            }

            Assert.IsTrue(luckyCheck.All(x=>x.Tornaments == 2));

        }

        private void OneTournamentAssertions(ContestentPool checkcontestents, int roundnumber)
        {
            int contestents = Convert.ToInt16(Math.Pow(2, roundnumber));
            int noncontestents = checkcontestents.Count() - contestents;
            Assert.AreEqual(contestents,checkcontestents.Where(x=>x.Tornaments==1).Count());
            Assert.AreEqual(noncontestents, checkcontestents.Where(x => x.Tornaments == 0).Count());
            Assert.AreEqual(checkcontestents.Count(), contestents + noncontestents);
            Assert.IsNotNull(checkcontestents.Where(x => x.TournementWins == 1).SingleOrDefault());
            Assert.AreEqual(checkcontestents.Count()-1,checkcontestents.Where(x=>x.TournementWins==0).Count());
            Assert.AreEqual(contestents - 1, checkcontestents.Sum(x => x.Wins));
            Assert.AreEqual(contestents - 1, checkcontestents.Sum(x => x.Losses));

            Assert.AreEqual(contestents-1, checkcontestents.Where(x => x.Losses == 1).Count());
            Assert.IsNotNull(checkcontestents.Where(x => x.Losses == 0 && x.Tornaments==1).SingleOrDefault());
            Assert.AreEqual(noncontestents+1,checkcontestents.Where(x => x.Losses==0).Count());
            Assert.AreEqual(contestents / 2, checkcontestents.Where(x => x.Wins == 0 && x.Tornaments == 1).Count());
            Assert.AreEqual(contestents/2 + noncontestents,checkcontestents.Where(x=>x.Wins==0).Count());
            Assert.IsNotNull(checkcontestents.Where(x => x.Wins == roundnumber).SingleOrDefault());

            for (int i=1;i<roundnumber;i++)
            {
                int iexpectedwins = Convert.ToInt32(Math.Pow(2, -(i+1)) * contestents);
                Assert.AreEqual(iexpectedwins, checkcontestents.Where(x => x.Wins==i).Count()); 
            }
            int expectedpoints = (roundnumber + 1) * (contestents / 4);
            int expectedwinnerpoints = contestents / 2;
            int expectedfinalpoints = contestents / 4;
            Assert.IsNotNull(checkcontestents.Where(x => x.Points == expectedwinnerpoints).SingleOrDefault());
            Assert.IsNotNull(checkcontestents.Where(x => x.Points == expectedwinnerpoints).SingleOrDefault());
          
            Assert.AreEqual(expectedpoints, checkcontestents.Sum(x => x.Points));

            for (int i = 1; i < roundnumber-1; i++)
            {
                int points = Convert.ToInt16(Math.Pow(2, i - 1));
                int iexpectedpoints = System.Convert.ToInt16((double)expectedfinalpoints * Math.Pow(0.5, i - 1));
                Assert.AreEqual(iexpectedpoints, checkcontestents.Where(x => x.Points==points).Count());
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

        static bool Random(Match m)
        {
            Random r = new Random();
            int randy = r.Next(100);
            return randy < 50;
        }
    }
}
