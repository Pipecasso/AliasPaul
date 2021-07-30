using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using AliasGeometry;
using Projector;

namespace Painter
{
    public class Canvas
    {
        private Bitmap _canvas;

        public Canvas(Bitmap bitty)
        {
            _canvas = bitty;
        }

        public Bitmap Bitmap { get => _canvas; }

        #region Transform
     
        public Point PointToPoint(Point2d p)
        {
            int ix = _canvas.Width / 2 + p.X;
            int iy = _canvas.Height / 2 - p.Y;
            return new Point(ix,iy);
        }

        public Tuple<Point,Point> LineToLine(Line2d l)
        {
            Point p1 = PointToPoint(l.start);
            Point p2 = PointToPoint(l.end);
            return new Tuple<Point, Point>(p1, p2);
        }

        public List<Point> EllipsebyPointToEllipseByPoint(Ellipse2dPointByPoint e)
        {
            List<Point> EllipseToGo = new List<Point>();
            foreach (Point2d p in e)
            {
                EllipseToGo.Add(PointToPoint(p));
            }
            return EllipseToGo;
        }

        public Rectangle EllipseToRectange(Ellipse2d e)
        {
            Point2d[] RectPoints = e.RectPoints();
            Point p1 = PointToPoint(RectPoints[0]);
            Point p4 = PointToPoint(RectPoints[3]);

            Point topleft = new Point(p1.X, p4.Y);
            Rectangle RectToGo = new Rectangle(topleft, new Size(e.rad1*2,e.rad2*2));
            return RectToGo;
        }

        
       

        

        #endregion


    }
}
