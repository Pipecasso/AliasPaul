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
        internal static Random _randy;

        public Match(Contestent first, Contestent second): base(first,second)
        {
            _result = Result.notplayed;
            if (_randy == null)
            {
                _randy = new Random();
            }    
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

        public Result result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }           
        }



        public double FlipProbability1(double randomfactor)
        {
            int p1 = Item1.Points;
            int p2 = Item2.Points;
            if (p1 == 0) p1++;
            if (p2 == 0) p2++;
            double flipprobability = randomfactor / 100;
            double totalpoints = Convert.ToDouble(p1+p2);
            double fflip = flipprobability * Convert.ToDouble(p1) / totalpoints;
            return fflip;
        }

        public double FlipProbability2(double randomfactor)
        {
            int p1 = Item1.Points;
            int p2 = Item2.Points;
            if (p1 == 0) p1++;
            if (p2 == 0) p2++;
            double flipprobability = randomfactor / 100;
            double totalpoints = Convert.ToDouble(p1 + p2);
            double fflip = flipprobability * Convert.ToDouble(p2) / totalpoints;
            return fflip;
        }

        public void RandomPlay(Result res,double randomfactor)
        {
            double fflip = res==Result.firstw ? FlipProbability1(randomfactor) : FlipProbability2(randomfactor);
            int flipside = Convert.ToInt32(Math.Floor(fflip * 100000)+0.5);
            bool flip = (_randy.Next(100000) + 1) <= flipside;
            if (flip)
            {
                result = res == Result.firstw ? Result.secondw : Result.firstw;
            }
            else
            {
                result = res;
            }
            
        }
    }
}
