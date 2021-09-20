using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    public class Cone2d
    {
        private Ellipse2dPointByPoint _start;
        private Ellipse2dPointByPoint _end;

        public Cone2d(Ellipse2dPointByPoint s, Ellipse2dPointByPoint e)
        {
            _start = s;
            _end = e;
        }

        public Ellipse2dPointByPoint start
        {
            get
            {
                return _start;
            }
        }

        public Ellipse2dPointByPoint end
        {
            get
            {
                return _end;
            }
        }

        public bool Single()
        {
            return _start.Center == _end.Center;
        }


    }
}
