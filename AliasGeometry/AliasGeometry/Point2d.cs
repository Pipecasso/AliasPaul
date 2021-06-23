using System;
using System.Collections.Generic;
using System.Text;

namespace AliasGeometry
{
    public class Point2d
    {
       private int _x;
       private int _y;
    
       public Point2d(int x,int y)
       {
            _x = x;
            _y = y;
       }

        public Point2d(double dx,double dy)
        {
            _x = Convert.ToInt32(Math.Floor(dx + 0.5));
            _y = Convert.ToInt32(Math.Floor(dy + 0.5));
        }

        public int X
        {
            get
            {
                return _x;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
        }

        public double dX
        {
            get
            {
                return Convert.ToDouble(_x);
            }

        }

        public double dY
        {
            get
            {
                return Convert.ToDouble(_y);
            }

        }

        public void Bound(ref int left, ref int right, ref int top, ref int bottom)
        {
            if (_x < left) left = _x;
            if (_x > right) right = _x;
            if (_y > top) top = _y;
            if (_y < bottom) bottom = _y;

        }

        public static double Distance(AliasGeometry.Point2d p, AliasGeometry.Point2d q)
        {
            return Math.Sqrt(Math.Pow((q.dX - p.dX), 2) + Math.Pow((q.dY - p.dY), 2));
        }
        

        public static void mxplusc(AliasGeometry.Point2d p, AliasGeometry.Point2d q,out double m,out double c)
        {
            if (p._y == q._y)
            {
                m = double.PositiveInfinity;
                c = p.dX;
            }
            else
            {
                m = (q.dY - p.dY) / (p.dY - p.dX);
                c = p.dY - m * p.dX;
            }
         }

        internal static void CheckBounds(Point2d p,ref int left,ref int right, ref int bottom,ref int top )
        {
            if (p.X < left) left = p.X;
            if (p.X > right) right = p.X;
            if (p.Y < bottom) bottom = p.Y;
            if (p.Y > top) top = p.Y;
        }

        public static Point2d MidPoint(Point2d p,Point2d q)
        {
            return new Point2d((p.dX + q.dX) / 2, (p.dY + q.dY) / 2);
        }

        public static Point2d Max(Point2d p,Point2d q)
        {
            return new Point2d(p.X > q.X ? p.X : q.X, p.Y > q.Y ? p.Y : q.Y);
        }

        public static Point2d Min(Point2d p, Point2d q)
        {
            return new Point2d(p.X < q.X ? p.X : q.X, p.Y < q.Y ? p.Y : q.Y);
        }

        public static Point2d Maxx(Point2d p, Point2d q)
        {
            return p.X > q.X ? p : q;
        }

        public static Point2d Minx(Point2d p, Point2d q)
        {
            return p.X < q.X ? p : q;
        }

        public static Point2d Maxy(Point2d p, Point2d q)
        {
            return p.Y > q.Y ? p : q;
        }

        public static Point2d Miny(Point2d p, Point2d q)
        {
            return p.Y < q.Y ? p : q;
        }

        public override bool Equals(object obj)
        {
            return obj is Point2d d &&
                   _x == d._x &&
                   _y == d._y;
        }

        public override int GetHashCode()
        {
            int hashCode = 979593255;
            hashCode = hashCode * -1521134295 + _x.GetHashCode();
            hashCode = hashCode * -1521134295 + _y.GetHashCode();
            return hashCode;
        }

        public static bool operator == (Point2d p,Point2d q)
        {
            return p.X == q.X && p.Y == q.Y;
        }

        public static bool operator != (Point2d p,Point2d q)
        {
            return !(p == q);
        }

    }
}
