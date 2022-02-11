using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupEngine
{
    public class Tournament
    {
        private Bag<Contestent> _contestentBag;
        private Match[] _Initial = new Match[16];
        private int _round;
        private int _match;


        public Tournament(ContestentPool contestentPool)
        {
            _round = 0;
            _match = 0;
            _contestentBag = new Bag<Contestent>();
            _contestentBag.Fill(contestentPool);

            for (int i = 0; i < 16; i++)
            {
                Match match = new Match(_contestentBag.Take(), _contestentBag.Take());
                _Initial[i] = match;
            }

        
        
        }
        
       
    
    }
}
