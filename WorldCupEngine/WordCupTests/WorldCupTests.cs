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
        public void ContestFromNew()
        {
            ContestentPool contestents = new ContestentPool("NewCelebs.xlsx", "Sheet1");
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
            contestents.Export("NewCelebs2.xlsx", "Tournament1", false);

            ContestentPool cpCheck = new ContestentPool("NewCelebs2.xlsx", "Tournament1");
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
            File.Copy("NewCelebs.xlsx", "CelebsCFNO.xlsx");
            ContestentPool contestents = new ContestentPool("CelebsCFNO.xlsx", "Sheet1");
            Tournament tournament = new Tournament(contestents, 5, false);
            PlayRandomTournamemt(tournament);
            contestents.Export("CelebsCFNO.xlsx", "Sheet1", true);

            ContestentPool cpCheck = new ContestentPool("CelebsCFNO.xlsx", "Sheet1");
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
            File.Copy("NewCelebs.xlsx", "NewCelebsTT.xlsx");
            ContestentPool contestents = new ContestentPool("NewCelebsTT.xlsx", "Sheet1");
            Tournament tournament = new Tournament(contestents, 5, false);
            PlayRandomTournamemt(tournament);
            contestents.Export("NewCelebsTT.xlsx", "Tournament1", false);
            contestents = new ContestentPool("NewCelebsTT.xlsx", "Tournament1");
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
            ContestentPool contestents = new ContestentPool("NewCelebs.xlsx", "Sheet1");
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
            ContestentPool contestents = new ContestentPool("NewCelebs.xlsx", "Sheet1");
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
            ContestentPool contestents = new ContestentPool("NewCelebs.xlsx", "Sheet1");
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

        [TestMethod]
        public void PauseAndSave()
        {
            File.Copy("NewCelebs.xlsx", "CelebsPS.xlsx");
            ContestentPool contestents = new ContestentPool("CelebsPS.xlsx", "Sheet1");
            Tournament tournament = new Tournament(contestents, 6, false);
            for (int i = 0; i < 20; i++)
            {
                PlayMatch(tournament.CurrentMatch, Random);
                tournament.NextMatch();
            }
            tournament.Save("PausedTournament2.xlsx");
            Tournament loadedTournament = new Tournament(contestents);
            loadedTournament.Load("PausedTournament2.xlsx");
     

            while (loadedTournament.CurrentMatch != null)
            {
                Match m = loadedTournament.CurrentMatch;
                PlayMatch(m, Random);
                loadedTournament.NextMatch();
            }

            contestents.Export("CelebsPS.xlsx", "Sheet1", true);

            ContestentPool cpCheck = new ContestentPool("CelebsPS.xlsx", "Sheet1");
            Assert.AreEqual(106, cpCheck.Count);
            Assert.IsNotNull(cpCheck.Where(x => x.TournementWins == 1).Single());
            Assert.AreEqual(105, cpCheck.Where(x => x.TournementWins == 0).Count());

            Assert.AreEqual(64, cpCheck.Where(x => x.Tornaments == 1).Count());
            Assert.AreEqual(42, cpCheck.Where(x => x.Tornaments == 0).Count());

            Assert.AreEqual(112, cpCheck.Sum(x => x.Points));

            Assert.AreEqual(63, cpCheck.Sum(x => x.Wins));
            Assert.AreEqual(63, cpCheck.Sum(x => x.Losses));

        }

        [TestMethod]
        public void MixUp()
        {
            ContestentPool contestents = new ContestentPool("NewCelebs.xlsx", "Sheet1");
            Tournament tournament = new Tournament(contestents, 5, false);
            Round round1 = tournament.CurrentRound;
            Match[] r1matches = round1.AllMatches.ToArray();
            PlayMatch(r1matches[0], Random);
            PlayMatch(r1matches[10], Random);
            PlayMatch(r1matches[3], Random);
            PlayMatch(r1matches[9], Random);
            PlayMatch(r1matches[2], Random);
            PlayMatch(r1matches[7], Random);
            PlayMatch(r1matches[1], Random);
            PlayMatch(r1matches[14], Random);
            PlayMatch(r1matches[15], Random);
            PlayMatch(r1matches[4], Random);
            PlayMatch(r1matches[12], Random);
            PlayMatch(r1matches[5], Random);
            PlayMatch(r1matches[6], Random);
            PlayMatch(r1matches[11], Random);
            PlayMatch(r1matches[8], Random);
            PlayMatch(r1matches[13], Random);
            Assert.AreEqual(0, round1.AllMatches.Where(x => x.result == Match.Result.notplayed).Count());
            Round round2 = tournament.NextRound();
            Assert.AreEqual(round2, tournament.CurrentRound);
            Match[] r2matches = round2.AllMatches.ToArray();
            PlayMatch(r2matches[0], Random);
            PlayMatch(r2matches[3], Random);
            PlayMatch(r2matches[2], Random);
            PlayMatch(r2matches[1], Random);
            PlayMatch(r2matches[4], Random);
            PlayMatch(r2matches[7], Random);
            PlayMatch(r2matches[6], Random);
            PlayMatch(r2matches[5], Random);
            Assert.AreEqual(0, round2.AllMatches.Where(x => x.result == Match.Result.notplayed).Count());
            Round round3 = tournament.NextRound();
            Assert.AreEqual(round3, tournament.CurrentRound);
            Match[] r3matches = round3.AllMatches.ToArray();
            PlayMatch(r3matches[0], Random);
            PlayMatch(r3matches[2], Random);

            tournament.Save("PausedTournament3.xlsx");
            Tournament loadedTournament = new Tournament(contestents);
            loadedTournament.Load("PausedTournament3.xlsx");
            Round currentround = loadedTournament.CurrentRound;
            Match[] currentMatches = currentround.AllMatches.ToArray();
            Assert.IsFalse(currentMatches[0].result == Match.Result.notplayed);
            Assert.IsTrue(currentMatches[1].result == Match.Result.notplayed);
            Assert.IsFalse(currentMatches[2].result == Match.Result.notplayed);
            Assert.IsTrue(currentMatches[3].result == Match.Result.notplayed);
            PlayMatch(currentMatches[3], Random);
            PlayMatch(currentMatches[1], Random);
            Round round4 = loadedTournament.NextRound();
            Assert.AreEqual(round4, loadedTournament.CurrentRound);
            Match[] r4Matches = round4.AllMatches.ToArray();
            PlayMatch(r4Matches[0], Random);
            PlayMatch(r4Matches[1], Random);
            Round final = loadedTournament.NextRound();
            Assert.AreEqual(final, loadedTournament.CurrentRound);
            Match finalm = final.AllMatches.First();
            PlayMatch(finalm, Random);
            Assert.IsNull(loadedTournament.NextRound());

            Assert.AreEqual(106, contestents.Count);
            Assert.IsNotNull(contestents.Where(x => x.TournementWins == 1).Single());
            Assert.AreEqual(105, contestents.Where(x => x.TournementWins == 0).Count());

            Assert.AreEqual(32, contestents.Where(x => x.Tornaments == 1).Count());
            Assert.AreEqual(74, contestents.Where(x => x.Tornaments == 0).Count());

            Assert.AreEqual(48, contestents.Sum(x => x.Points));

            Assert.AreEqual(31, contestents.Sum(x => x.Wins));
            Assert.AreEqual(31, contestents.Sum(x => x.Losses));
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
            int randy = r.Next(100);
            return randy < 50;
        }
    }
}
