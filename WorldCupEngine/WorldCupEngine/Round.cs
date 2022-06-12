using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace WorldCupEngine
{
    public class Round
    {
        private Match[] _matches;
        private int _number;
        private int _heat;

        internal Round(int number,IXLCell cell,IEnumerable<Contestent> contestent_pool)
        {
            _number = number;
            Load(cell, contestent_pool);
        }

        public Round(IEnumerable<Contestent> initialContestents)
        {
            _number = 1;
            _heat = 0;

            int matchcount = initialContestents.Count() / 2;
            _matches = new Match[matchcount];
            IEnumerable<Match> matches = MatchMaker(initialContestents, false);
            int i = 0;
            foreach (Match m in matches)
            {
                _matches[i] = m;
                i++;
            }
        }

        public Round(IEnumerable<Contestent> seeds,IEnumerable<Contestent> qualifiers)
        {
            _number = 1;
            _heat = 0;
            Contestent[] qarray = qualifiers.ToArray();
            int index = 0;
            _matches = new Match[seeds.Count()];
            foreach (Contestent con in seeds)
            {
                Match m = new Match(con, qarray[index]);
                _matches[index++] = m;
            }
        }

        public Round(int number,IEnumerable<Match> matches)
        {
            _number = number;
            _heat = 0;
            _matches = new Match[matches.Count()];
            int i = 0;

            foreach (Match m in matches)
            {
                _matches[i] = m;
                i++;
            }
        }
        public int RoundNumber
        {
            get => _number;
        }

        public IEnumerable<Match> AllMatches
        {
            get
            {
                return _matches;
            }
        }

        public Match CurrentMatch
        {
            get
            {
                Match m = null;
                if (_heat<_matches.Length)
                {
                    m = _matches[_heat];
                }
                return m;
            }
        }

        public bool NextMatch()
        {
            _heat++;
            return _heat < _matches.Length;
        }

        private IEnumerable<Match> MatchMaker(IEnumerable<Contestent> contestents,bool facup)
        {
            List<Match> matches = new List<Match>();
            if (facup)
            {
                Bag<Contestent> nextlot = new Bag<Contestent>();
                nextlot.Fill(contestents);
                matches = new List<Match>();
                while (!nextlot.Empty)
                {
                    Match m = new Match(nextlot.Take(), nextlot.Take());
                    matches.Add(m);
                }
            }
            else
            {
                IEnumerator<Contestent> contestentenum = contestents.GetEnumerator();
                bool keegoing = true;
                matches = new List<Match>();
                while (keegoing)
                {
                    keegoing = contestentenum.MoveNext();
                    if (keegoing)
                    {
                        Contestent c1 = contestentenum.Current;
                        contestentenum.MoveNext();
                        Contestent c2 = contestentenum.Current;

                        Match m = new Match(c1, c2);
                        matches.Add(m);
                    }
                }
            }
            return matches;
        }

        public Round Next(bool facup)
        {
            Round nextround = null;
            IEnumerable<Contestent> winners = _matches.Select(x => x.Winner());
            if (winners.Count() > 1)
            {
                IEnumerable<Match> matches = MatchMaker(winners, facup);
                nextround = new Round(_number + 1, matches);
            }
            return nextround;
        }

        public IEnumerable<Contestent> GetContestents()
        {
            List<Contestent> contestents = new List<Contestent>();
            foreach(Match m in _matches)
            {
                contestents.Add(m.Item1);
                contestents.Add(m.Item2);
            }
            return contestents;
        }

        public bool IsFinal { get => _matches.Count() == 1; }

     
            
     
        private void Load(IXLCell celltop,IEnumerable<Contestent> cpool)
        {
            IXLCell cellmatches = celltop.CellRight();
            int matches = cellmatches.GetValue<int>();
            IXLCell cellheat = cellmatches.CellBelow();
            _heat = cellheat.GetValue<int>();
            _matches = new Match[matches];
            IXLCell player2cell = cellheat.CellBelow();
            IXLCell player1cell = player2cell.CellLeft();
            for (int i=1;i<=matches;i++)
            {
                string player1 = player1cell.GetValue<string>();
                string player2 = player2cell.GetValue<string>();
                Contestent c1 = cpool.Single(x => x.Name == player1);
                Contestent c2 = cpool.Single(x => x.Name == player2);
                Match m = new Match(c1, c2);

                if (player1cell.Style.Fill.BackgroundColor == XLColor.AppleGreen)
                {
                    m.result = Match.Result.firstw;
                }
                else if (player2cell.Style.Fill.BackgroundColor == XLColor.AppleGreen)
                {
                    m.result = Match.Result.secondw;
                }
                else
                {
                    m.result = Match.Result.notplayed;
                }
                _matches[i - 1] = m;
                player1cell = player1cell.CellBelow();
                player2cell = player2cell.CellBelow();
            }
        }

        internal void Save(IXLCell coltop)
        {
            IXLCell col1 = coltop;
            IXLCell col2 = coltop.CellRight();
            col1.SetValue<string>("Matches");
            col2.SetValue<int>(_matches.Length);
            col1 = col1.CellBelow();
            col1.SetValue<string>("Heat");
            col2 = col2.CellBelow();
            col2.SetValue<int>(_heat);
            col1 = col1.CellBelow();
            col2 = col2.CellBelow();
            foreach (Match m in _matches)
            {
                col1.SetValue<string>(m.Item1.Name);
                col2.SetValue<string>(m.Item2.Name);
                if (m.result != Match.Result.notplayed)
                {
                    IXLCell wincell = m.result == Match.Result.firstw ? col1 : col2;
                    wincell.Style.Fill.BackgroundColor = XLColor.AppleGreen;
                }
                col1 = col1.CellBelow();
                col2 = col2.CellBelow();
            }
        }
    
    
    }
}
