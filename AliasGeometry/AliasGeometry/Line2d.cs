using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{

    public class Vector2d : Tuple<double,double>
    { 
        public Vector2d(double d1,double d2)  : base(d1,d2)
        {
        }

        public Vector2d() : base(0, 0) { }

        public Vector2d(Point2d p1, Point2d p2) : base(p2.dY - p1.dY, p2.dX - p1.dX)
        {
        }

        public double Magnitude()
        {
            return Math.Sqrt(Item1 * Item1 + Item2 * Item2);
        }

        public static Vector2d Normalise(Vector2d vin)
        {
            double mag = vin.Magnitude();
            return new Vector2d(vin.Item1 / mag, vin.Item2 / mag);

        }

        public static Vector2d operator + (Vector2d one, Vector2d two)
        {
            return( new Vector2d(one.Item1 + two.Item1, one.Item2 + two.Item2));
        }

        public static Vector2d operator -(Vector2d one, Vector2d two)
        {
            return (new Vector2d(one.Item1 - two.Item1, one.Item2 - two.Item2));
        }

       

    
     
    };

     
    public class Line2d
    {
        Point2d _start;
        Point2d _end;

        public Line2d(Point2d start,Point2d end)
        {
            _start = start;
            _end = end;
        }

        public Point2d start
        {
            get
            {
                return _start;
            }
        }

        public Point2d end
        {
            get
            {
                return _end;
            }

        }

        public double m()
        {
            if (end.X == start.X)
            {
                return end.Y >= start.Y ? double.PositiveInfinity : double.NegativeInfinity;
            }
            else
            {
                
                return (end.dY - start.dY) / (end.dX - start.dX);
             }
        }

        public double c()
        {
            double m = this.m();
            if (double.IsInfinity(m))
            {
                return double.PositiveInfinity;
            }
            else
            {
                return end.dY - m * end.dX;
            }
        }
        /*
        public static bool Intersection(Line2d l1,Line2d l2,ref double dx,ref double dy)
        {
            bool ret;
            if (Math.Abs(l1.m()) == Math.Abs(l2.m()))
            {
                ret = false;
            }
            else
            {
                ret = true;
                dx = (l2.c() - l1.c()) / (l1.m() - l2.m());
                dy = l1.m() * dx + l1.c();
            }
            return ret;
        }
       
        public static bool Intersection(Line2d l1,Line2d l2,out Point2d i)
        {
            double dx = 0;
            double dy = 0;
            bool ret = Intersection(l1, l2, ref dx, ref dy);
            if (ret)
            {
                i = new Point2d(dx, dy);
            }
            else
            {
                i = null;
            }
            return ret;
        }*/

        public double DistanceFromExtendedLine(Point2d p)
        {
            double grad = m();
            double m1 = grad == 0 ? double.PositiveInfinity : -1 / grad;
            double c1 = double.IsInfinity(m1) ? double.PositiveInfinity : p.Y - m1 * p.X;
            double dx = 0;
            double dy = 0;
            Point2d intersection;


            if (double.IsInfinity(m1))
            {
                intersection = new Point2d(p.X, start.Y);
            }
            else if (double.IsInfinity(grad))
            {
                intersection = new Point2d(start.X, p.Y);
            }
            else
            {
                double intercept = c();
                dx = (c1 - intercept) / (grad - m1);
                dy = grad * dx + intercept;
                intersection = new Point2d(dx, dy);
            }
            return Point2d.Distance(p, intersection);
        }


        #region Geometry

        public static bool Intersection(Line2d l1,Line2d l2,out Point2d intersection)
        {
            double tolerance = 0.001;
            if (Parallel(l1,l2))
            {
                intersection = null;
                return false;
            }
            else
            {
                double dx, dy;
                if (l1.IsVertical() || l2.IsVertical())
                {
                    //x of vertical
                    if (l1.IsVertical())
                    {
                        dx = l1.start.X;
                        dy = l2.m() * dx + l2.c();
                    }
                    else
                    {
                        dx = l2.start.X;
                        dy = l1.m() * dx + l1.c();
                    }
                }
                else
                {
                    double l1m =l1.m();
                    double l1c = l1.c();
                    dx = (l2.c() - l1.c()) / (l1.m() - l2.m());
                    dy = l1.m() * dx + l1.c();
                }
                intersection = new Point2d(dx, dy);
                return l1.IsPointOnLine(dx,dy, tolerance) && l2.IsPointOnLine(dx,dy, tolerance);
            }

        }

        public static bool Parallel(Line2d l1, Line2d l2)
        {
            return Math.Abs(l1.m()) == Math.Abs(l2.m());
        }

        bool IsPointOnExtendedLine(double dx,double dy, double dTolerance)
        {
            //we are going to use the perpendicular distance from a point (Check.x,Check.y) to the line x1y1 to x2y2
            // where: x1y1 == ( 2DStart.x(), 2DStart.y() )
            // and	  x2y2 == ( 2DEnd.x(), 2DEnd.y() )
            // Math notation -> distance = | ( (y1-y2)Check.x - (x1-x2)Check.y + x1y2 - x2y1 ) |
            //							  -------------------------------------------------------
            //										(sqrt( (x1-x2)^2 + (y1-y2)^2 ) )

            double dPerpDistanceFromPointToLine;
            double dNumerator = Math.Abs((_start.Y - _end.Y) * (dx) - (_start.X - _end.X) * (dy) + (_start.X * _end.Y) - (_end.X * _start.Y));
            double dDenominator = Math.Sqrt(Math.Pow(_start.X - _end.X, 2) + Math.Pow(_start.Y - _end.Y, 2));
            dPerpDistanceFromPointToLine = dNumerator / dDenominator;
           
            if (dDenominator == 0.0)
            {
                return false;
            }
            else
            {
                if (Math.Abs(dPerpDistanceFromPointToLine) <= dTolerance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsPointOnLine(double dx,double dy, double dTolerance)
        {
            if (IsPointOnExtendedLine(dx,dy, dTolerance) == true)
            {
                double dSmallX, dLargeX, dSmallY, dLargeY;
                // check if the Y coordinate is between start and end of line y points
                if (_end.Y > _start.Y)
                {
                    dSmallY = _start.Y - dTolerance;
                    dLargeY = _end.Y + dTolerance;
                }
                else
                {
                    dSmallY = _end.Y - dTolerance;
                    dLargeY = _start.Y + dTolerance;
                }

                if (_end.X > _start.X)
                {
                    dSmallX = _start.X - dTolerance;
                    dLargeX = _end.X + dTolerance;
                }
                else
                {
                    dSmallX = _end.X - dTolerance;
                    dLargeX = _start.X + dTolerance;
                }

                if ((dx >= dSmallX) && (dx <= dLargeX) &&
                    (dy >= dSmallY) && (dy <= dLargeY))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }



        public bool IsVertical()
        {
            return _start.X == _end.X;
        }
#endregion

        public double Length()
        {
            return Point2d.Distance(_start, _end);
        }

        public void Bound(ref int left,ref int right,ref int top,ref int bottom)
        {
            _start.Bound(ref left, ref right, ref top, ref bottom);
            _end.Bound(ref left, ref right, ref top, ref bottom);
        }
    
    }
}
