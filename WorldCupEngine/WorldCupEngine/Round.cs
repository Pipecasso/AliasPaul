using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupEngine
{
    public class Round
    {
        private Match[] _matches;
        private int _number;
        private int _heat;

        

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

        /* public IEnumerable<Match> CurrentMatch()
         {
             Match togo = null;
             int current = _heat;

             if (current < _matches.Length)
             {
                 _heat++;
                 togo = _matches[current];
             }
             yield return togo;
         }*/

        public Match CurrentMatch
        {
            get
            {
                return _matches[_heat];
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
                contestentenum.Reset();
                bool keegoing = true;
                matches = new List<Match>();
                while (keegoing)
                {
                    Contestent c1 = contestentenum.Current;
                    contestentenum.MoveNext();
                    Contestent c2 = contestentenum.Current;
                    keegoing = contestentenum.MoveNext();
                    Match m = new Match(c1, c2);
                    matches.Add(m);
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
    
    
    }
}
