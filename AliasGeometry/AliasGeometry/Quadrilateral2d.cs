using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{

    public enum RectalProbeLineResult
    {
        Unset,
        Boundary,
        Contained,
        CompletelyOutside,
        EndsOutside
    };

    
    public class Quadrilateral2d
    {
        public Quadrilateral2d(Point2d a, Point2d b, Point2d c, Point2d d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        //a  //b
        //c  //d

        //points are stored in traditional cartesian.

        public Point2d a { get; protected set; }
        public Point2d b { get; protected set; }
        public Point2d c { get; protected set; }
        public Point2d d { get; protected set; }
    
    }

    public class Rectangle2d : Quadrilateral2d
    {
        public Rectangle2d(Point2d a, Point2d d) : base(a, new Point2d(d.X, a.Y), new Point2d(a.X, d.Y),d)
        {
        }

        public double Width
        {
            get
            {
                return b.X - a.X;
            }
        }

        public double Height
        {
            get
            {
                return Point2d.Distance(this.a, this.b);
            }
        
        }

        public bool IsPointInside(Point2d p)
        {
            return a.X <= p.X && p.X <= b.X && c.X <= p.X && p.X <= d.X
                && c.Y <= p.Y && p.Y <= a.Y && d.Y <= p.Y && p.Y <= b.Y;
        }

        public RectalProbeLineResult BoundaryIntersection(Line2d l,out Point2d intersection)
        {
            RectalProbeLineResult rectalProbeLineResult = RectalProbeLineResult.Unset;


            //intersection
            Point2d i;
            List<Point2d> intersections = new List<Point2d>();
            if (Line2d.Intersection(l, Left, out i)) intersections.Add(i);
            if (Line2d.Intersection(l, Right, out i)) intersections.Add(i);
            if (Line2d.Intersection(l, Top, out i)) intersections.Add(i);
            if (Line2d.Intersection(l, Bottom, out i)) intersections.Add(i);
            
            if (intersections.Count ==0)
            {
                intersection = null;
                rectalProbeLineResult = IsPointInside(l.start) && IsPointInside(l.end) ? RectalProbeLineResult.Contained : RectalProbeLineResult.CompletelyOutside;
            }
            else if (intersections.Count == 1)
            {
                rectalProbeLineResult = RectalProbeLineResult.Boundary;
                intersection = intersections[0];
            }
            else
            {
                rectalProbeLineResult = RectalProbeLineResult.EndsOutside;
                intersection = null;
            }

            return rectalProbeLineResult;
        }

        public Point2d Centre()
        {
            return Point2d.MidPoint(a, d);
        }

        public Line2d Top
        {
            get
            {
                return new Line2d(a, b);
            }
        }

        public Line2d Bottom
        {
            get
            {
                return new Line2d(c, d);
            }
        }

        public Line2d Left
        { 
            get
            {
                return new Line2d(a, c);
            }
        }

        public Line2d Right
        {
            get
            {
                return new Line2d(b, d);
            }
        }





    }

}
