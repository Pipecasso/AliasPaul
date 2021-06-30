using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace AliasGeometry
{

    public class EllipseEnumerator : IEnumerator
    {
        private int _tick;
        private Ellipse2dPointByPoint _ellipse;
        private Point2d _point;

        public EllipseEnumerator(Ellipse2dPointByPoint e)
        {
            _tick = -1;
            _ellipse = e;
        }
    
        public object Current { get => _point; }
        public bool MoveNext()
        {
            bool ret;
            if (_tick < (_ellipse.PointCount - 1))
            {
                _tick++;
                _point = _ellipse[_tick];
                ret = true;
            }
            else
            {
                ret = false;
            }
            return ret;
        }

        public void Reset()
        {
            _tick = -1;
            _point = null;
        }
    
    
    }



    public class Ellipse2dPointByPoint : Ellipse2d, IEnumerable
    {
        private Point2d[] _PointByPoint;
        private int _PointCount;


        public Ellipse2dPointByPoint(Point2d ptcenter, double rad1, double rad2,int pointcount = 360) : base(ptcenter, rad1, rad2)
        {
            _PointCount = pointcount;
            _PointByPoint = new Point2d[_PointCount];
        }


        public Ellipse2dPointByPoint(Point2d ptcenter, int pointcount = 360) : base(ptcenter)
        {
            _PointByPoint = new Point2d[_PointCount];
        }

        public Point2d this[int i]
        {
            get
            {
                return _PointByPoint[i];
            }

            set
            {
                _PointByPoint[i] = value;
            }
        }

        public int PointCount { get => _PointCount; }

        public void DerriveRadii()
        {
            _rad1 = double.MinValue;
            _rad2 = double.MaxValue;

            //play it safe do all the points
            for (int i = 0; i < _PointByPoint.Length; i++)
            {
                Point2d p = _PointByPoint[i];
                double distance = Point2d.Distance(_ptCentre, p);
                if (distance > _rad1)
                {
                    _rad1 = distance;
                }

                if (distance < _rad2)
                {
                    _rad2 = distance;
                }
            }

        }

        public IEnumerator GetEnumerator()
        {
            return new EllipseEnumerator(this);
        }


    }
}
