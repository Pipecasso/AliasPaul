using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliasGeometry;

namespace Projector
{
    public class Shapes3d
    {
        private List<Line3d> _Lines;
        private List<Cone3d> _Cones;

        public Shapes3d()
        {
            _Lines = new List<Line3d>();
            _Cones = new List<Cone3d>();
        }

        public List<Line3d> Lines { get => _Lines; }
        public List<Cone3d> Cones { get => _Cones; }

        public List<Point3d> Points()
        {
            Action<List<Point3d>, Circle3dPointByPoint> mycircleaction = (PointsToAdd, circle) =>
            {
                for (int i=0;i<circle.TotalPoints;i++)
                {
                    PointsToAdd.Add(circle[i]);
                }
            };
            
            List<Point3d> pointList = new List<Point3d>();

            foreach (Line3d line3d in _Lines)
            {
                pointList.Add(line3d.P);
                pointList.Add(line3d.Q);
            }

            foreach(Cone3d cone3d in _Cones)
            {
                mycircleaction(pointList, cone3d.circleStart);
                mycircleaction(pointList, cone3d.circleEnd);
            }

            return pointList;
        }
    
    }
}
