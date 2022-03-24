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
            ContestentPool contestents = new ContestentPool("Celebs.xlsx", "Sheet1");
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
            ContestentPool contestents = new ContestentPool("Celebs.xlsx", "Sheet1");
            Tournament tournament = new Tournament(contestents, 5, false);
            PlayRandomTournamemt(tournament);
            Assert.IsNotNull(tournament.Winner());

            Assert.IsNotNull(contestents.Where(x => x.TournementWins == 1).Single());
            Assert.AreEqual(105, contestents.Where(x => x.TournementWins == 0).Count());

            Assert.AreEqual(32, contestents.Where(x => x.Tornaments == 1).Count());
            Assert.AreEqual(74, contestents.Where(x => x.Tornaments == 0).Count());

            Assert.AreEqual(48, contestents.Sum(x => x.Points));

            Assert.AreEqual(31, contestents.Sum(x => x.Wins));
            Assert.AreEqual(31, contestents.Sum(x => x.Losses));
            contestents.Export("Celebs2.xlsx","Tournament1",false);

            ContestentPool cpCheck = new ContestentPool("Celebs2.xlsx", "Tournament1",false);
            Assert.AreEqual(106, cpCheck.Count);
            Assert.IsNotNull(cpCheck.Where(x => x.TournementWins == 1).Single());
            Assert.AreEqual(105, cpCheck.Where(x => x.TournementWins == 0).Count());

            Assert.AreEqual(32, cpCheck.Where(x => x.Tornaments == 1).Count());
            Assert.AreEqual(74, cpCheck.Where(x => x.Tornaments == 0).Count());

            Assert.AreEqual(48, cpCheck.Sum(x => x.Points));

            Assert.AreEqual(31, cpCheck.Sum(x => x.Wins));
            Assert.AreEqual(31, cpCheck.Sum(x => x.Losses));

        }

        [TestMethod]
        public void ContestFromNewOverwrite()
        {
            ContestentPool contestents = new ContestentPool("Celebs.xlsx", "Sheet1");
            Tournament tournament = new Tournament(contestents, 5, false);
            PlayRandomTournamemt(tournament);
            contestents.Export("Celebs.xlsx", "Sheet1", true);

            ContestentPool cpCheck = new ContestentPool("Celebs.xlsx", "Sheet1", false);
            Assert.AreEqual(106, cpCheck.Count);
            Assert.IsNotNull(cpCheck.Where(x => x.TournementWins == 1).Single());
            Assert.AreEqual(105, cpCheck.Where(x => x.TournementWins == 0).Count());

            Assert.AreEqual(32, cpCheck.Where(x => x.Tornaments == 1).Count());
            Assert.AreEqual(74, cpCheck.Where(x => x.Tornaments == 0).Count());

            Assert.AreEqual(48, cpCheck.Sum(x => x.Points));

            Assert.AreEqual(31, cpCheck.Sum(x => x.Wins));
            Assert.AreEqual(31, cpCheck.Sum(x => x.Losses));

        }

        [TestMethod]
        public void TwoTournaments()
        {
            ContestentPool contestents = new ContestentPool("Celebs.xlsx", "Sheet1");
            Tournament tournament = new Tournament(contestents, 5, false);
            PlayRandomTournamemt(tournament);
            contestents.Export("Celebs22.xlsx", "Tournament1", false);
            contestents = new ContestentPool("Celebs22.xlsx", "Tournament1",false);
            tournament = new Tournament(contestents, 5, false);
            PlayRandomTournamemt(tournament);
            Assert.AreEqual(2, contestents.Sum(x => x.TournementWins));
            Assert.AreEqual(64, contestents.Sum(x => x.Tornaments));
            Assert.AreEqual(96, contestents.Sum(x => x.Points));
            Assert.AreEqual(62, contestents.Sum(x => x.Wins));
            Assert.AreEqual(62, contestents.Sum(x => x.Losses));
        }

        [TestMethod]
        public void PlayOneRound()
        {
            ContestentPool contestents = new ContestentPool("Celebs.xlsx", "Sheet1");
            Tournament tournament = new Tournament(contestents, 5, false);
            Round firstRound = tournament.CurrentRound;
            while (tournament.CurrentRound == firstRound)
            {
                Match m = tournament.CurrentMatch;
                PlayMatch(m, Random);
                tournament.NextMatch();
            }

            Assert.IsTrue(contestents.All(x => (x.Wins == 1 || x.Wins == 0)));
            Assert.IsTrue(contestents.All(x => (x.Losses == 1 || x.Losses == 0)));
            Assert.IsFalse(contestents.Any(x=> (x.Wins == 1 && x.Losses==1)));
            Assert.AreEqual(16, contestents.Sum(x => x.Wins));
            Assert.AreEqual(16, contestents.Sum(x => x.Losses));

        }

        [TestMethod]
        public void PauseTournament()
        {
            ContestentPool contestents = new ContestentPool("Celebs.xlsx", "Sheet1");
            Tournament tournament = new Tournament(contestents, 5, false);
            for (int i=0;i<20;i++)
            {
                PlayMatch(tournament.CurrentMatch, Alphatetic);
                tournament.NextMatch();
            }
            tournament.Save("PausedTournament.xlsx");
            Tournament loadedTournament = new Tournament(contestents);
            loadedTournament.Load("PausedTournament.xlsx");
            Assert.AreEqual(2,loadedTournament.CurrentRound.RoundNumber);
            Assert.AreEqual(Match.Result.notplayed,loadedTournament.CurrentRound.CurrentMatch.result);
            
            while (loadedTournament.CurrentMatch != null)
            {
                Match m = loadedTournament.CurrentMatch;
                PlayMatch(m, Alphatetic);
                loadedTournament.NextMatch();
            }

            while (tournament.CurrentMatch != null)
            {
                Match m = tournament.CurrentMatch;
                PlayMatch(m, Alphatetic);
                tournament.NextMatch();
            }

            Assert.IsNotNull(loadedTournament.Winner());
            Assert.AreEqual(tournament.Winner(), loadedTournament.Winner());

            Assert.IsNotNull(contestents.Where(x => x.TournementWins == 2).SingleOrDefault());
            Assert.AreEqual(105, contestents.Where(x => x.TournementWins == 0).Count());

            Assert.AreEqual(32, contestents.Where(x => x.Tornaments == 1).Count());
            Assert.AreEqual(74, contestents.Where(x => x.Tornaments == 0).Count());

            Assert.AreEqual(96, contestents.Sum(x => x.Points));
        }

        [TestMethod]
        public void PauseTournamenEndRound()
        {
            ContestentPool contestents = new ContestentPool("Celebs.xlsx", "Sheet1");
            Tournament tournament = new Tournament(contestents, 5, false);
            for (int i = 0; i < 16; i++)
            {
                PlayMatch(tournament.CurrentMatch, Alphatetic);
                tournament.NextMatch();
            }
            tournament.Save("PausedTournament.xlsx");
            Tournament loadedTournament = new Tournament(contestents);
            loadedTournament.Load("PausedTournament.xlsx");
            Assert.AreEqual(2, loadedTournament.CurrentRound.RoundNumber);
            Assert.AreEqual(Match.Result.notplayed, loadedTournament.CurrentRound.CurrentMatch.result);

            while (loadedTournament.CurrentMatch != null)
            {
                Match m = loadedTournament.CurrentMatch;
                PlayMatch(m, Alphatetic);
                loadedTournament.NextMatch();
            }

            Assert.IsNotNull(loadedTournament.Winner());
            
            Assert.IsNotNull(contestents.Where(x => x.TournementWins == 1).SingleOrDefault());
            Assert.AreEqual(105, contestents.Where(x => x.TournementWins == 0).Count());

            Assert.AreEqual(32, contestents.Where(x => x.Tornaments == 1).Count());
            Assert.AreEqual(74, contestents.Where(x => x.Tornaments == 0).Count());

            Assert.AreEqual(48, contestents.Sum(x => x.Points));
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
