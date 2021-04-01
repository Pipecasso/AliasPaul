using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AliasGeometry
{
    public class Vector3d
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public enum Axis
        {
            none = 0,
            ew,
            ns,
            ud,
            skewed
        
        };


        public Vector3d(double dx, double dy, double dz)
        {
            X = dx;
            Y = dy;
            Z = dz;
        }

        public Vector3d(Point3d p,Point3d q)
        {
            X = q.X - p.X;
            Y = q.Y - p.Y;
            Z = q.Z - p.Z;
        }

        public Vector3d(Vector3d v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

      

        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public void Normalise()
        {
            double m = Magnitude();
            X /= m;
            Y /= m;
            Z /= m;
        }

        public static Vector3d Normalise(Vector3d v)
        {
            double m = v.Magnitude();
            return new Vector3d(v.X / m, v.Y / m, v.Z / m);

        }

        public static double Dot(Vector3d v1,Vector3d v2)
        {
            return (v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z);
        }

        public static Vector3d CrossProduct(Vector3d v1,Vector3d v2)
        {
            double dx = (v1.Y * v2.Z) - (v1.Z * v2.Y);
            double dy = (v1.Z * v2.X) - (v1.X * v2.Z);
            double dz = (v1.X * v2.Y) - (v1.Y * v2.X);
            Vector3d vToGo = new Vector3d(dx, dy, dz);
            //double tTest1 = Vector3d.Dot(vToGo, v1);
            //double tTest2 = Vector3d.Dot(vToGo, v2);
            //Debug.Assert(Math.Abs(tTest1) < 1e-6 && Math.Abs(tTest2) < 1e-6);
            return vToGo;
        }

        public static Vector3d operator * (Vector3d v,double d)
        {
            return new Vector3d(v.X * d,v.Y*d,v.Z * d);
        }

        public static Vector3d operator + (Vector3d v1, Vector3d v2)
        {
            return new Vector3d(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3d operator -(Vector3d v1, Vector3d v2)
        {
            return new Vector3d(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public Axis GetAxis()
        { 
            int iAxis = 0;
            const int east = 1;
            const int north = 2;
            const int up = 4;
            Axis ax = Axis.none;

            if (Math.Abs(X) > 1e-6)
            {
                iAxis = iAxis | east;
            }

            if (Math.Abs(Y) > 1e-6)
            {
                iAxis = iAxis | north;
            }
            if (Math.Abs(Z) > 1e-6)
            {
                iAxis = iAxis | up;
            }
            if (iAxis > 0)
            {
                switch (iAxis)
                {
                    case east: ax = Axis.ew; break;
                    case north: ax = Axis.ns;break;
                    case up: ax = Axis.ud;break;
                    default:ax = Axis.skewed;break;
                }
            }
            return ax;
        }

        public bool Orthogonal()
        {
            Vector3d vtest = Normalise(this);
            double d = Math.Abs(Dot(vtest, new Vector3d(1, 0, 0)));
            d+= Math.Abs(Dot(vtest, new Vector3d(0, 1, 0)));
            d += Math.Abs(Dot(vtest, new Vector3d(0, 0, 1)));
            return (Math.Abs(d - 1) < 1e-7);
        }



    }
}
