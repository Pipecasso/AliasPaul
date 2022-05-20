using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoFilter
{
    public class PulseArgs : EventArgs
    {
        double _total;
        double _current;
        public PulseArgs(int total)
        {
            _total = total;
            _current = 0;
        }
       
        public int Current
        {
            get
            {
                return System.Convert.ToInt32(_current);
            }
            set
            {
                _current = value;
            }
        }

        public double PercentComplete()
        {
            return  _current/_total*100;
        }

    
    }
}
