using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    public class Plane3d
    {
        protected Vector3d _N;
        protected Point3d _P;

        public Plane3d(Vector3d v,Point3d p)
        {
            _N = Vector3d.Normalise(v);
            _P = p;
        }

        internal Plane3d(Vector3d v)
        {
            _N = v;
        }

        internal Plane3d()
        {}

        public Point3d P { get => _P; internal set { _P = value; } }

        public Vector3d N { get => _N; internal set { _N = value; } }

        public bool Intersection(Point3d Q, Vector3d d,ref Point3d I)
        {
            Vector3d vn = Vector3d.Normalise(d);
            if (Math.Abs(Vector3d.Dot(vn,_N)) < 1e-6)
            {
                return false;
            }
            else
            {
                //make a right angle triangle with Q,P and R where R intersection of the plane and Q + N
                Vector3d vHypotenuse = new Vector3d(Q, _P);
                double hypotenuse = vHypotenuse.Magnitude();
                vHypotenuse.Normalise();
                double PQR = Vector3d.Dot(vHypotenuse, _N);
                if (Math.Abs(PQR) < 1e-6)
                {
                    //its already on the plane
                    I = Q;
                    return true;
                }
                else
                {
                    double QR = hypotenuse * PQR;
                    Point3d R = Q + _N * QR;
                    //make another right angle triangle with QRI
                    double IQR = Vector3d.Dot(_N,vn);
                    double QI = QR / IQR;
                    I = Q + vn * QI;
                    return true;
                }
            }
        }

        public bool IsPointOnPlane(Point3d P,double tolerance = double.Epsilon)
        {
            Vector3d vplane = new Vector3d(_P, P);
            vplane.Normalise();
            double dp = Vector3d.Dot(vplane, _N);
            return Math.Abs(dp) < tolerance;
        }

        public Point3d NearestPoint(Point3d point)
        {
            Point3d pout;
            if (IsPointOnPlane(point))
            {
                pout = point;
            }
            else
            {
                pout = new Point3d();
                Intersection(point, _N, ref pout);
            }
            return pout;
        }


       


  
    }
}
