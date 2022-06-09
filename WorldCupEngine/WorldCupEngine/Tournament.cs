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
        public enum Format { standard,facup,seeded};
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

        public Tournament(ContestentPool contestentPool,int numberofrounds,Format format,int seed)
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
            _format= format;
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

                Round round1 = null;
                if (_format == Format.seeded)
                {
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
                
                foreach (KeyValuePair<int,Round> pair in _Rounds)
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
                if (nextRound!=null) { current = nextRound.CurrentMatch; }
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

        public void Update(string path,string sheetname)
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
            IXLCell label=worksheet.Cell("A1");
            label.SetValue<string>("Rounds");
            IXLCell labelright = label.CellRight();
            labelright.SetValue<int>(_Rounds.Count);
            IXLCell coltop = worksheet.Cell("A2");
            foreach (KeyValuePair<int,Round> kvp in _Rounds)
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
            IXLWorkbook workbook = new XLWorkbook(path);
            IXLWorksheet worksheet = workbook.Worksheet("Tournament");
            IXLCell totalrounds = worksheet.Cell("B1");
            _round = totalrounds.GetValue<int>();
            IXLCell labelright = worksheet.Cell("A2");
            int round = labelright.GetValue<int>();
            IXLCell coltop = worksheet.Cell("A3");
            for (int i=0;i<_round;i++)
            {
                Round r = new Round(i+1, coltop, _allPlayers);
                _Rounds.Add(i+1, r);
                coltop = coltop.CellRight().CellRight();
            }
        }

        public Format RoundFormat
        {   
            get { return _format; }
            set { _format = value; }
        }
    }
}
