using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupEngine
{
    public class Tournament
    {

        private int _round;
        private int _match;
        private Dictionary<int, Round> _Rounds;
     

        private bool _facup;


        public Tournament(ContestentPool contestentPool, int numberofrounds, bool facup)
        {
            _round = 1;
            _match = 0;
            _facup = facup;
            Bag<Contestent> contestentBag = new Bag<Contestent>();
            contestentBag.Fill(contestentPool);
            _Rounds = new Dictionary<int, Round>();
            int contestent_total = Convert.ToInt32(Math.Pow(2, numberofrounds));
            if (contestent_total <= contestentPool.Count)
            {
                List<Contestent> contestents = new List<Contestent>();
                for (int i = 0; i < contestent_total; i++)
                {
                    contestents.Add(contestentBag.Take());
                }

                Round round1 = new Round(contestents);
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

        public Match NextMatch()
        {
            Match current = null;
            if (_Rounds[_round].NextMatch())
            {
                current = _Rounds[_round].CurrentMatch;
            }
            else
            {
                Round nextRound = _Rounds[_round].Next(_facup);
                if (nextRound != null)
                {
                    _round++;
                    _Rounds.Add(_round, nextRound);
                    current = nextRound.CurrentMatch;
                }
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
        
       
    
    }
}
