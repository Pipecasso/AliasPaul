using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace WorldCupEngine
{
    public class Tournament
    {
        public enum Format { standard, facup, seeded };
        private int _round;
        private Dictionary<int, Round> _Rounds;
        private ContestentPool _allPlayers;



        private Format _format;

        public Tournament(ContestentPool contestentPool)
        {
            _allPlayers = contestentPool;
            _Rounds = new Dictionary<int, Round>();
            _format = Format.standard;
        }

        public Tournament(ContestentPool contestentPool, int numberofrounds, Format format, int seed)
        {
            _round = 1;
            _allPlayers = contestentPool;
            _format = format;
            Bag<Contestent> contestentBag = new Bag<Contestent>();
            contestentBag.Randy = new Random(seed);
            Initialise(contestentBag, contestentPool, numberofrounds);
        }

        public Tournament(ContestentPool contestentPool, int numberofrounds, Format format)
        {
            _round = 1;
            _allPlayers = contestentPool;
            _format = format;
            Bag<Contestent> contestentBag = new Bag<Contestent>();
            Initialise(contestentBag, contestentPool, numberofrounds);
        }

        private void Initialise(Bag<Contestent> contestentBag, ContestentPool contestentPool, int numberofrounds)
        {
            contestentBag.Fill(contestentPool);
            _Rounds = new Dictionary<int, Round>();
            int contestent_total = Convert.ToInt32(Math.Pow(2, numberofrounds));
            if (contestent_total <= contestentPool.Count)
            {
                List<Contestent> contestents = new List<Contestent>();
                for (int i = 0; i < contestent_total; i++)
                {
                    Contestent contestent = contestentBag.Take();
                    contestents.Add(contestent);
                }
                Seed(contestents);
                Round round1 = null;
                if (_format == Format.seeded)
                {
                    int half = Convert.ToInt32(Math.Pow(2, numberofrounds - 1));
                    IEnumerable<Contestent> seededContestens = contestents.OrderBy(x => x.Seeding);
                    IEnumerable<Contestent> tophalf = seededContestens.Take(half);
                    IEnumerable<Contestent> bottomhalf = contestents.Where(x => tophalf.All(y => y.Name != x.Name));
                    IEnumerable<Contestent> shuffletop = SeedShuffle(tophalf);
                    round1 = new Round(shuffletop, bottomhalf);

                }
                else
                {
                    round1 = new Round(contestents);
                }
                _Rounds.Add(1, round1);
            }
        }

        public Match CurrentMatch
        {
            get
            {
                return _Rounds[_round].CurrentMatch;
            }
        }

        public Round CurrentRound
        {

            get
            {
                return _Rounds[_round];
            }
        }

        public IEnumerable<Contestent> Contestents
        {
            get
            {
                List<Contestent> Contestents = new List<Contestent>();
                Round round1 = _Rounds[1];
                foreach (Match m in round1.AllMatches)
                {
                    Contestents.Add(m.Item1);
                    Contestents.Add(m.Item2);
                }
                return Contestents;
            }
        }

        public Round NextRound()
        {
            Round nextRound = _Rounds[_round].Next(_format == Format.facup);
            if (nextRound == null)
            {
                Round final_round = _Rounds[_round];
                Match final = final_round.AllMatches.Single<Match>();
                final.Winner().TournamentWin();
                int winner_points = Convert.ToInt32(Math.Pow(2, (double)(_round - 1)));
                final.Winner().AddPoints(winner_points);
                foreach (Contestent contestent in Contestents) contestent.Picked();

                foreach (KeyValuePair<int, Round> pair in _Rounds)
                {
                    int roundnum = pair.Key;
                    Round current = pair.Value;
                    IEnumerable<Contestent> Winners = current.AllMatches.Select(x => x.Winner());
                    IEnumerable<Contestent> Losers = current.AllMatches.Select(x => x.Loser());
                    foreach (Contestent winner in Winners)
                    {
                        winner.IncWin();
                    }
                    foreach (Contestent loser in Losers)
                    {
                        loser.IncLoss();
                        if (_round > 1)
                        {
                            int points = Convert.ToInt32(Math.Pow(2, (double)(roundnum - 2)));
                            loser.AddPoints(points);
                        }
                    }
                }
            }
            else
            {
                _round++;
                _Rounds.Add(_round, nextRound);
            }
            return nextRound;
        }

        public Match NextMatch()
        {
            Match current = null;
            if (_Rounds[_round].NextMatch())
            {
                current = _Rounds[_round].CurrentMatch;
            }
            else
            {
                Round nextRound = NextRound();
                if (nextRound != null) { current = nextRound.CurrentMatch; }
            }
            return current;
        }

        public Contestent Winner()
        {
            Contestent winner = null;
            Round current = _Rounds[_round];
            if (current.AllMatches.Count() == 1 && current.AllMatches.First().result != Match.Result.notplayed)
            {
                winner = current.AllMatches.First().Winner();
            }
            return winner;
        }

        public void Update(string path, string sheetname)
        {
            IXLWorkbook wb = new XLWorkbook(path);
            if (sheetname == String.Empty) sheetname = _allPlayers.SheetName;
            IXLWorksheet ws = wb.Worksheet(sheetname);
            IXLColumn column = ws.ColumnsUsed().FirstOrDefault();

            foreach (Contestent contestent in Contestents)
            {
                IXLRow row = column.Cells().Where(x => x.GetString() == contestent.Name).First().WorksheetRow();

                IXLCell picked = row.Cell("B");
                picked.SetValue<int>(contestent.Tornaments);

                IXLCell champion = row.Cell("C");
                champion.SetValue<int>(contestent.TournementWins);

                IXLCell wins = row.Cell("D");
                wins.SetValue<int>(contestent.Wins);

                IXLCell losses = row.Cell("E");
                losses.SetValue<int>(contestent.Losses);

                IXLCell points = row.Cell("F");
                points.SetValue<int>(contestent.Points);

            }
            wb.Save();
        }

        public void Save(string path)
        {
            IXLWorkbook workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.AddWorksheet("Tournament");
            IXLCell label = worksheet.Cell("A1");
            label.SetValue<string>("Rounds");
            IXLCell labelright = label.CellRight();
            labelright.SetValue<int>(_Rounds.Count);
            IXLCell coltop = worksheet.Cell("A2");
            foreach (KeyValuePair<int, Round> kvp in _Rounds)
            {
                Round round = kvp.Value;
                coltop.SetValue<int>(kvp.Key);
                IXLCell firstContestent = coltop.CellBelow();
                coltop = coltop.CellRight().CellRight();
                round.Save(firstContestent);
            }
            workbook.SaveAs(path);
        }

        public void Load(string path)
        {
            Seed(_allPlayers);
            IXLWorkbook workbook = new XLWorkbook(path);
            IXLWorksheet worksheet = workbook.Worksheet("Tournament");
            IXLCell totalrounds = worksheet.Cell("B1");
            _round = totalrounds.GetValue<int>();
            IXLCell labelright = worksheet.Cell("A2");
            int round = labelright.GetValue<int>();
            IXLCell coltop = worksheet.Cell("A3");
            for (int i = 0; i < _round; i++)
            {
                Round r = new Round(i + 1, coltop, _allPlayers);
                _Rounds.Add(i + 1, r);
                coltop = coltop.CellRight().CellRight();
            }
        }

        public Format RoundFormat
        {
            get { return _format; }
            set { _format = value; }
        }

        private void Seed(IEnumerable<Contestent> contestents)
        {
            IEnumerable<Contestent> sortedContestent_pool = contestents.OrderByDescending(x => x.Points).ThenBy(x => x.TournementWins).ThenBy(x => x.WinPercentage).ThenBy(x => x.Wins).ThenBy(x => x.Name);
            uint seed = 1;
            foreach (Contestent contestent in sortedContestent_pool)
            {
                contestent.Seeding = seed++;
            }
        }

        List<int> SeedList(List<int> listin,bool b64)
        {
            List<int> togo;
            if (b64)
            {
                togo = new List<int>() { listin[0] + 1, listin[1] - 1, listin[2] - 1, listin[3] + 1, listin[4] - 1, listin[5] + 1, listin[6] + 1, listin[7] - 1 };
            }
            else
            {
                togo = new List<int>() { listin[0] + 1, listin[1] - 1, listin[2] - 1, listin[3] + 1 };
            }
            return togo;
        }


        private IEnumerable<Contestent> SeedShuffle(IEnumerable<Contestent> contestents)
        {

            List<int> l1;
            List<int> l2;
            List<int> l3;
            List<int> l4;

            if (contestents.Count() == 32)
            {
                l1 = new List<int>() { 1, 32, 16, 17, 8, 25, 9, 24 };
                l2 = SeedList(l1,true);
                l3 = SeedList(l2, true);
                l4 = SeedList(l3, true);
            }
            else
            {
                l1 = new List<int>() { 1, 16, 8, 9 };
                l2 = SeedList(l1, false);
                l3 = SeedList(l2, false);
                l4 = SeedList(l3, false);
            }

            List<Contestent> seeds = new List<Contestent>();
            Contestent[] carray = contestents.ToArray();
            IEnumerable<int> tophalf = l1.Concat(l3.Reverse<int>());
            IEnumerable<int> bottomhalf = (l2.Concat(l4.Reverse<int>())).Reverse();

            IEnumerable<int> finalseeds = tophalf.Concat(bottomhalf);
            foreach (int fs in finalseeds)
            {
                seeds.Add(carray[fs-1]);
            }

            /*Contestent[] conarray = contestents.ToArray();
            Contestent[] seededconarray = new Contestent[conarray.Length];

            int top = 0;
            int bottom = contestents.Count() - 1;
            int lowermid = contestents.Count() / 2;
            int uppermid = lowermid - 1;
            
            for (int i = 0; i < conarray.Length/2;i++)
            {
                int j = conarray.Length - 1 - i;
                Contestent c1 = conarray[i];
                Contestent c2 = conarray[j];

                switch (i % 4)
                {
                    case 0:
                        //top
                        seededconarray[top++] = c1;
                        seededconarray[top++] = c2;
                        break;
                    case 1:
                        //bottom
                        seededconarray[bottom--] = c1;
                        seededconarray[bottom--] = c2;
                        break;
                    case 2:
                        //uppermid
                        seededconarray[uppermid--] = c1;
                        seededconarray[uppermid--] = c2;
                        break;
                    case 3:
                        //lowermid
                        seededconarray[lowermid++] = c1;
                        seededconarray[lowermid++] = c2;
                        break;                 
                }
            }
        }*/
            //return seededconarray;
            return seeds;
        }
    }
}
