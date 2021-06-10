using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    static class ListExtension
    {
        public static Point3d Average<T>(this IList<Point3d> list)
        {
            Point3d sum = list.Aggregate((acc, cur) => acc + cur);
            double d = list.Count;
            Point3d av = sum / d;
            return av;
        }
    }

    
    public class Point3dCluster : Dictionary<Point3d,List<Point3d>>
    {
       public Point3dCluster(double tolerance)
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
                pointslist = new List<Point3d>();
                pointslist.Add(p);
            }

            Point3d paverage = pointslist.Average<Point3d>();
            if (this.ContainsKey(keypoint))
            {
                this.Remove(keypoint);
            }
            base.Add(keypoint, pointslist);
        }
        
        private Point3d AveragePoint(List<Point3d> points)
        {
            Point3d sum = points.Aggregate((acc, cur) => acc + cur);
            double count = Convert.ToDouble(points.Count);
            Point3d average = sum / count;
            return average;
        }

    }
}
