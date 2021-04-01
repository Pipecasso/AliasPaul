using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    public class Circle3d
    {
        private Point3d _ptCenter;
        private double _radius;
        private Vector3d _up;
        private Vector3d _across;
        private Vector3d _Normal;


        private Point3d[] _PointByPoint;     //this might comein handy!!



        public Circle3d(Point3d Center, double radius, Vector3d vnormal, Vector3d vup, Vector3d vacross)
        {
            _ptCenter = Center;
            _radius = radius;
            _up = vup;
            _across = vacross;
            _Normal = vnormal;
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

        public Point3d Polar(double theta)
        {

            int Quadrant = 0;
            if (theta >= 0 && theta < Math.PI / 2)
            {
                Quadrant = 1;
            }
            else if (theta >= Math.PI / 2 && theta < Math.PI)
            {
                Quadrant = 2;
            }
            else if (theta >= Math.PI && theta < Math.PI * 1.5)
            {
                Quadrant = 3;
            }
            else
            {
                Quadrant = 4;
            }

            double updistance = _radius * Math.Cos(theta);
            double acrossdistance = _radius * Math.Sin(theta);
            Vector3d vFinal = _up * updistance + _across * acrossdistance;

            Point3d vout = _ptCenter + vFinal;
            return vout;
        }

        public Point3d Center
        {
            get
            {
                return _ptCenter;
            }
        }

        public double radius
        {
            get
            {
                return _radius;
            }
        }

        public Point3d this[int i]
        {
            get
            {
                return _PointByPoint[i];
            }
        }

        public Vector3d Normal
        {
            get
            {
                return _Normal;
            }
        }       

      
    }
}
