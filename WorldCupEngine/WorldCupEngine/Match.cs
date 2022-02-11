using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupEngine
{
    public class Match : Tuple<Contestent,Contestent>
    {
        public enum Result { notplayed,firstw,secondw};
        private Result _result;

        public Match(Contestent first, Contestent second): base(first,second)
        {
            _result = Result.notplayed;
        }

        public Contestent Winner()
        {
            Contestent togo = null;
            if (_result != Result.notplayed)
            {
                togo = _result == Result.firstw ? Item1 : Item2;
            }
            return togo;
        }

        public Contestent Loser()
        {
            Contestent togo = null;
            if (_result != Result.notplayed)
            {
                togo = _result == Result.firstw ? Item2 : Item1;
            }
            return togo;
        }
        




    }
}
