using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    public class BoundedPlane3d : Plane3d
    {
       

        private Point3d _TopLeft;
        private Point3d _BottomRight;
        private Point3d _TopRight;
        private Point3d _BottomLeft;
       

        public BoundedPlane3d(Vector3d v,Point3d topleft,Point3d bottomright,Point3d topright,Point3d bottomleft) : base(v)
        {
            _TopLeft = topleft;
            _BottomRight = bottomright;
            _TopRight = topright;
            _BottomLeft = bottomleft;
            base.P = CenterPoint();
        }

        public bool IsValid(double tolerance = double.Epsilon)
        {
            bool goodtogo1 = false;
            bool goodtogo2 = false;

            Vector3d vtop = new Vector3d(_TopLeft, _TopRight);
            Vector3d vbottom = new Vector3d(_BottomLeft, _BottomRight);
            Vector3d vleft = new Vector3d(_TopLeft, _BottomLeft);
            Vector3d vright = new Vector3d(_TopRight, _BottomRight);

            Point3d WalkPoint = _TopLeft + vtop + vright - vbottom - vleft;
            Vector3d Error = new Vector3d(_TopLeft, WalkPoint);
            goodtogo1 = Error.Magnitude() <= tolerance;
            if (goodtogo1)
            {
                vtop.Normalise();
                vright.Normalise();
                goodtogo2 = Math.Abs(Vector3d.Dot(vtop, vright)) <= tolerance && Math.Abs(Vector3d.Dot(vtop, _N)) <= tolerance && Math.Abs(Vector3d.Dot(vright, _N)) <= tolerance;
            }
            return goodtogo2;
        }


        public new bool IsPointOnPlane(Point3d P)
        {
            bool ret = false;
            if (base.IsPointOnPlane(P))
            {
                Rectangle2d cartestianrect = CartesianRectange();
                Point2d cartesianpoint = CartiesianPoint(P);
                ret = cartestianrect.IsPointInside(cartesianpoint);
            }
            return ret;
        }

        private Point3d CenterPoint()
        {
            Vector3d Top = new Vector3d(_TopLeft, _TopRight);
            double toplength = Top.Magnitude();
            Top.Normalise();
            Vector3d Right = new Vector3d(_TopRight, _BottomRight);
            double rightlength = Right.Magnitude();
            Right.Normalise();

            Point3d Centre = _TopLeft + Top * (toplength / 2) + Right * (rightlength / 2);
            return Centre;
        }



        public Line3d Top
        {
            get
            {
                return new Line3d(_TopLeft, _TopRight);
            }
        }

        public Line3d Bottom
        {
            get
            {
                return new Line3d(_BottomLeft, _BottomRight);
            }
        
        }

        public Line3d Left
        { 
            get
            {
                return new Line3d(_TopLeft, _BottomLeft);
            }
        
        }

        public Line3d Right
        { 
            get
            {
                return new Line3d(_TopRight, _BottomRight);
            }
        }

        public Rectangle2d CartesianRectange()
        {
            double width = Point3d.Distance(_TopLeft, _TopRight);
            double height = Point3d.Distance(_TopRight, _BottomRight);
            Point2d topleft = new Point2d(-width / 2, height / 2);
            Point2d bottomright = new Point2d(width / 2, -height / 2);
            return new Rectangle2d(topleft, bottomright);
        }

        public Point2d CartiesianPoint(Point3d cartesianp)
        {
            Point2d pret = null;
            if (base.IsPointOnPlane(cartesianp, 1e-10) )
            {
                Vector3d t = new Vector3d(_TopLeft,_TopRight);
                t.Normalise();
                Vector3d r = new Vector3d(_BottomRight,_TopRight);
                r.Normalise();

                Vector3d diagonal = new Vector3d(_P, cartesianp);

                Equation e1 = new Equation(t.X, r.X, _N.X, diagonal.X);
                Equation e2 = new Equation(t.Y, r.Y, _N.Y, diagonal.Y);
                Equation e3 = new Equation(t.Z, r.Z, _N.Z, diagonal.Z);

                Equation3 equation3 = new Equation3(e1, e2, e3);
                double dx = 0;
                double dy = 0;
                double dz = 0;
                bool solved = equation3.Solve(ref dx, ref dy, ref dz);
                if (solved)
                {
                    //it has to be and dz must be zero
                    pret = new Point2d(dx, dy);
                }
            }
            return pret;
        }


    }
}
