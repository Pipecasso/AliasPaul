using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliasGeometry;

namespace Projector
{
    public class Shapes2d
    {
        private List<Line2d> _Lines;
        private List<Cone2d> _Cones;
        private List<TextLabel2d> _TextLabels;


    
        //currently these are for diagnostic purposes only
        private Dictionary<Line3d, Line2d> _LineMap;
        private Dictionary<Cone3d, Cone2d> _ConeMap;



        public Shapes2d()
        {
            _Lines = new List<Line2d>();
            _Cones = new List<Cone2d>();
            _TextLabels = new List<TextLabel2d>();
            _LineMap = new Dictionary<Line3d, Line2d>();
            _ConeMap = new Dictionary<Cone3d, Cone2d>();




        }



        public void AddLine(Line2d l, Line3d l3)
        {

            _Lines.Add(l);
            _LineMap.Add(l3, l);
        }

        public void AddCone(Cone2d c, Cone3d c3)
        {
            _Cones.Add(c);
            _ConeMap.Add(c3, c);
        }

        public void AddTextLabel(TextLabel2d t)
        {
            _TextLabels.Add(t);
        }

    

        Rectangle2d BoundingRectangle()
        {
            
            
            
            List<Point2d> Points = new List<Point2d>();
            foreach (Line2d l in _Lines)
            {
                Points.Add(l.start);
                Points.Add(l.end);
            }

            foreach (Cone2d c in _Cones)
            {
                Ellipse2dPointByPoint start = c.start;
                Ellipse2dPointByPoint end = c.end;
                foreach (Point2d p in start) Points.Add(p);
                foreach (Point2d p in end) Points.Add(p);
            }

            foreach (TextLabel2d textlab in _TextLabels)
            {
                Points.Add(textlab.Location);
            }

            Point2d topleft = Points.Aggregate((acc, cur) => Point2d.CartesianTopLeft(acc, cur));
            Point2d bottomright = Points.Aggregate((acc, cur) => Point2d.CartesianBottomRight(acc, cur));
            return new Rectangle2d(topleft, bottomright);


        }

        public List<Line2d> Lines
        {
            get
            {
                return _Lines;
            }
        }

        public List<Cone2d> Cones
        {
            get
            {
                return _Cones;
            }
        }

        public List<TextLabel2d> Text { get { return _TextLabels; } }

      /*  public bool DoesPointClash(Point2d point)
        {
            return point.X >= _left && point.X <= _right &&
                point.Y >= _bottom && point.Y <= _top;
        }*/

        internal Dictionary<Line3d, Line2d> LineMap
        {
            get
            {
                return _LineMap;
            }
        }

        internal Dictionary<Cone3d, Cone2d> ConeMap
        {
            get
            {
                return _ConeMap;
            }
        }

 

    }



}

