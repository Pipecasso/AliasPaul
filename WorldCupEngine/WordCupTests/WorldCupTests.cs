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
            Tournament t = new Tournament(contestents, 6, false);
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
            OneTournamentAssertions(checkcontestents,6);

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

            for (int i=roundnumber;i>=1;i--)
            {
                int winloss = roundnumber - i + 1;
                int round_total = Convert.ToInt16(Math.Pow(2, i-1));
                Assert.AreEqual(round_total, checkcontestents.Where(x => x.Wins == winloss).Count());
                Assert.AreEqual(round_total, checkcontestents.Where(x => x.Losses == winloss).Count());
                if (i>1 && i<roundnumber)
                {
                    int pointsinround = Convert.ToInt32(Math.Pow(2, i -2));
                    int expectednumber = round_total / 2;
                    Assert.AreEqual(expectednumber, checkcontestents.Where(x => x.Points == pointsinround).Count());
                }
                    
             }
            int expectedpoints = (roundnumber - 1) * contestents / 2 + contestents;
            int expectedwinnerpoints = contestents / 2;
            int expectedfinalpoints = contestents / 4;
            Assert.IsNotNull(checkcontestents.Where(x => x.Points == expectedwinnerpoints).SingleOrDefault());
            Assert.IsNotNull(checkcontestents.Where(x => x.Points == expectedwinnerpoints).SingleOrDefault());
          
            Assert.AreEqual(expectedpoints, checkcontestents.Sum(x => x.Points));
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
