using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCupEngine;




namespace WordCupTests
{
    [DeploymentItem("..\\..\\..\\Celebs.xlsx")]

    [TestClass]
    public class WorldCupTests
    {
        [TestMethod]
        public void ContestentPoolNew()
        {
            ContestentPool contestents = new ContestentPool("Celebs.xlsx", "Sheet1", true);
            Assert.AreEqual(106, contestents.Count);
            Assert.IsTrue(contestents.All(x => x.Tornaments == 0));
            Assert.IsTrue(contestents.All(x => x.Points == 0));
            Assert.IsTrue(contestents.All(x => x.TournementWins == 0));
            Assert.IsTrue(contestents.All(x => x.Losses == 0));
            Assert.IsTrue(contestents.All(x => x.Wins == 0));
        }

        [TestMethod]
        public void ContestFromNew()
        {
            ContestentPool contestents = new ContestentPool("Celebs.xlsx", "Sheet1", true);
            Tournament tournament = new Tournament(contestents, 5, false);
            PlayRandomTournamemt(tournament);
            Assert.IsNotNull(tournament.Winner());

            Assert.IsNotNull(contestents.Where(x => x.TournementWins == 1).Single());
            Assert.AreEqual(105, contestents.Where(x => x.TournementWins == 0).Count());

            Assert.AreEqual(32, contestents.Where(x => x.Tornaments == 1).Count());
            Assert.AreEqual(74, contestents.Where(x => x.Tornaments == 0).Count());

            Assert.AreEqual(48, contestents.Sum(x => x.Points));

            Assert.AreEqual(62, contestents.Sum(x => x.Wins));
            Assert.AreEqual(62, contestents.Sum(x => x.Losses));
        }

        static void PlayRandomTournamemt(Tournament t)
        {
            while (t.CurrentMatch != null)
            {
                Match m = t.CurrentMatch;
                PlayMatch(m, Random);
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

        static bool Random(Match m)
        {
            Random r = new Random();
            return r.Next(1, 3) == 1;
        }


    }
}
