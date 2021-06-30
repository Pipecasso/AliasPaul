using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    public class Ellipse2d
    {
        protected Point2d _ptCentre;
        protected double _rad1;
        protected double _rad2;
       

        public Ellipse2d(Point2d ptcenter, double rad1, double rad2)
        {
            _ptCentre = ptcenter;
            _rad1 = rad1;
            _rad2 = rad2;
        }

        public Ellipse2d(Point2d ptcenter)
        {
            _ptCentre = ptcenter;
            _rad1 = 0;
            _rad2 = 0;
        }

        public Point2d Center
        {
            get
            {
                return _ptCentre;
            }
        }

        public int rad1
        {
            get
            {
                return Convert.ToInt32(Math.Floor(_rad1 + 0.5));
            }
        }

        public int rad2
        {
            get
            {
                return Convert.ToInt32(Math.Floor(_rad2 + 0.5));
            }
        }

    


        public Point2d[] RectPoints()
        {
            Point2d p1 = new Point2d(_ptCentre.X - rad1, _ptCentre.Y);
            Point2d p2 = new Point2d(_ptCentre.X + rad1, _ptCentre.Y);
            Point2d p3 = new Point2d(_ptCentre.X, _ptCentre.Y - rad2);
            Point2d p4 = new Point2d(_ptCentre.X, _ptCentre.Y + rad2);

            Point2d[] edges = new Point2d[] { p1, p2, p3, p4 };
            return edges;
        }
        
       

    }
}
