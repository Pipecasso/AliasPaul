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
        public int TotalPoints { get; private set; }

        public Circle3dPointByPoint(Point3d Center, double radius, Vector3d vnormal, Vector3d vup, Vector3d vacross,int totalPoints = 360) : base(Center,radius,vnormal,vup,vacross)
        {
            TotalPoints = totalPoints;
            WorkPolar();
        }

        private void WorkPolar()
        {
            _PointByPoint = new Point3d[TotalPoints];
            for (int i = 0; i < TotalPoints; i++)
            {
                double deg = (i / TotalPoints) * 360; 
                double irad = (deg / 180.0) * Math.PI;
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
