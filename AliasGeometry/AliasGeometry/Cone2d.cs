using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    public class Cone2d
    {
        private Ellipse2d _start;
        private Ellipse2d _end;

        public Cone2d(Ellipse2d s,Ellipse2d e)
        {
            _start = s;
            _end = e;
        }

        public Ellipse2d start
        {
            get
            {
                return _start;
            }
        }

        public Ellipse2d end
        {
            get
            {
                return _end;
            }
        }


    }
}
