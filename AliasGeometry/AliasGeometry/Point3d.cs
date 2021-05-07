using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace AliasGeometry
{
    public class Point3d
    {
        private double _x;
        private double _y;
        private double _z;

        public Point3d()
        {
            _x = 0;
            _y = 0;
            _z = 0;
        }

 
        public Point3d(double dx, double dy, double dz)
        {
            _x = dx;
            _y = dy;
            _z = dz;
        }

        public double X
        {
            get
            {
                return _x;
            }

            set
            {
                _x = value;
            }
        }

        public double Y
        {
            get
            {
                return _y;
            }

            set
            {
                _y = value;
            }
        }

        public double Z
        {
            get
            {
                return _z;
            }

            set
            {
                _z = value;
            }
        }

        static public Vector3d operator - (Point3d p,Point3d q)
        {
           Vector3d pout = new Vector3d(p.X-q.X,p.Y - q.Y,p.Z - q.Z);
            return pout;
        }

        static public Point3d operator +(Point3d p, Vector3d q)
        {
            Point3d pout = new Point3d(p.X + q.X, p.Y + q.Y, p.Z + q.Z);
            return pout;
        }

        static public Point3d operator -(Point3d p, Vector3d q)
        {
            Point3d pout = new Point3d(p.X - q.X, p.Y - q.Y, p.Z - q.Z);
            return pout;
        }

        static public double Distance(Point3d p, Point3d q)
        {
            Vector3d vW = p - q;
            return vW.Magnitude();
		}

        static public Point3d MidPoint(Point3d p, Point3d q)
        {
            return new Point3d((p.X + q.X) / 2, (p.Y + q.Y) / 2, (p.Z + q.Z) / 2);
        }

        public void Set(double x,double y,double z)
        {
            _x = (float)(x);
            _y = (float)(y);
            _z = (float)(z);
        }
    }

   


}
