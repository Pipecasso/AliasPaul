using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    public class Line3d
    {
        private Point3d _p;
        private Point3d _q;

        public Line3d(Point3d p,Point3d q)
        {
            _p = p;
            _q = q;
        }

        public Point3d P
        {
            get
            {
                return _p;
            }
        }

        public Point3d Q
        {
            get
            {
                return _q;
            }
        }

        public double Length()
        {
            return Point3d.Distance(_p, _q);
        }

    }
}
