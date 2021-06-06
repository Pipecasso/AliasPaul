using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{

    public class LineCubeIntersection
    {
        public enum Intersection
        {
            Unset,
            Miss,
            Touch,
            PassThrough,
            Surface
        };

        private Intersection _intersection;
        private Dictionary<Face, Point3d> _faceMap;
        private Dictionary<Point3d, int> _pointMap;
        private List<Face> _surfaceFaces;
        private Point3d _refpoint;

        public LineCubeIntersection(Point3d refpoint)
        {
            _faceMap = new Dictionary<Face, Point3d>();
            _pointMap = new Dictionary<Point3d, int>();
            _surfaceFaces = new List<Face>();
            _intersection = Intersection.Unset;
            _refpoint = refpoint;

        }

        public Intersection intersection { get => _intersection; }
        public List<Face> SurfaceFaces { get => _surfaceFaces; set { _surfaceFaces = value; } }
        public Dictionary<Face,Point3d> FaceMap { get => _faceMap; }
        public Dictionary<Point3d,int> PointMap { get => _pointMap; }

        public Line3d IntersectionLine()
        {
            Line3d intersectLine = null;
            if (_intersection == Intersection.PassThrough || _intersection == Intersection.Surface)
            {
                Point3d p1 = null;
                Point3d p2 = null;
                foreach (KeyValuePair<Point3d,int> kvp in _pointMap)
                {
                    if (p1 == null) p1 = kvp.Key;
                    else if (p2 == null) p2 = kvp.Key;
                }

                double d1 = Point3d.Distance(_refpoint, p1);
                double d2 = Point3d.Distance(_refpoint, p2);

                if (d1 < d2)
                {
                    intersectLine = new Line3d(p1, p2);
                }
                else
                {
                    intersectLine = new Line3d(p2, p1);
                }
            }
            return intersectLine;
        }

        public void Add(Face face, Point3d p)
        {
            _faceMap.Add(face, p);
            if (_pointMap.ContainsKey(p))
            {
                _pointMap[p]++;
            }
            else
            {
                _pointMap.Add(p, 1);
            }
        }

     

        public void Determine()
        { 
            if (_surfaceFaces.Count == 0)
            {
                //0 we are good as we are
                //1,5, >6 Euclid says this is impossibe
                if (_faceMap.Count() == 2)
                {
                    if (_pointMap.Count == 1)
                    {
                        _intersection = Intersection.Touch;
                    }
                    else
                    {
                        _intersection = Intersection.PassThrough;
                    }
                }
                else if (_faceMap.Count() == 3)
                {
                    _intersection = Intersection.Touch;
                }
                else if (_faceMap.Count() == 4 || _faceMap.Count() == 6)
                {
                    _intersection = Intersection.PassThrough;
                }
            }
            else
            {
                _intersection = Intersection.Surface;
            }
        }

        

    }
}
