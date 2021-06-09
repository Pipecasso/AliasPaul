using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    public class Point3dCluster : Dictionary<Point3d,List<Point3d>>
    {
        Point3dCluster(double tolerance)
        {
            Tolerance = tolerance;
        }
        public double Tolerance { get;}
    
        public void Add(Point3d p)
        {
            Point3d keypoint = null;
            List<Point3d> pointslist = null;
            if (ContainsKey(p))
            {
                pointslist = this[p];
                keypoint = p;
            }
            else
            {
                foreach (KeyValuePair<Point3d,List<Point3d>> kvp in this)
                {
                    Point3d representitive = kvp.Key;
                    if (Point3d.NearlyEquals(p,representitive,Tolerance))
                    {
                        pointslist = kvp.Value;
                        keypoint = kvp.Key;
                        break;
                    }
                }
            }

            if (pointslist == null)
            {
                pointslist.Add(p);
            }

            Point3d paverage = AveragePoint(pointslist);
            this.Remove(keypoint);
            base.Add(keypoint, pointslist);
        }

        private static void AddPoint(Point3d p1,Point3d p2)
        {
            p1.X += p2.X;
            p1.Y += p2.Y;
            p1.Z += p2.Z;
        }

       
        private Point3d AveragePoint(List<Point3d> points)
        {
            Point3d sum = points.Aggregate((acc, cur) => acc + cur);
            double count = Convert.ToDouble(points.Count);
            Point3d average = points.Aggregate((acc, cur) => (acc + cur) / count);
            return average;
        }

    }
}
