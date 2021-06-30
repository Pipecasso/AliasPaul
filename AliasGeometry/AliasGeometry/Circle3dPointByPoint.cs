using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    public class Circle3dPointByPoint : Circle3d
    {
        private Point3d[] _PointByPoint;

        public Circle3dPointByPoint(Point3d Center, double radius, Vector3d vnormal, Vector3d vup, Vector3d vacross) : base(Center,radius,vnormal,vup,vacross)
        {
            WorkPolar();
        }

        private void WorkPolar()
        {
            _PointByPoint = new Point3d[360];
            for (int i = 0; i < 360; i++)
            {
                double irad = (i / 180.0) * Math.PI;
                _PointByPoint[i] = Polar(irad);
            }
        }

        public Point3d this[int i]
        {
            get
            {
                return _PointByPoint[i];
            }
        }


    }
}
