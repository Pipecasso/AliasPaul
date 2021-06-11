using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using AliasGeometry;


namespace AliasGeometryFunctionalTests
{

    public static class MyExtensions
    {

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            Random r = new Random();
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }



    [TestClass]
    public class Point3dTests
    {
        private List<Point3d> _Points;
        
        public Point3dTests()
        {
            _Points = ALotOfPoints.GetSomePoints();
        }

        [TestMethod]
        public void AveragePoint()
        {
            Point3d sum = _Points.Aggregate((acc, cur) => acc + cur);
            double count = Convert.ToDouble(_Points.Count);
            Point3d average = sum / count;

            Point3d TargetPointAverage = new Point3d(36.12765957, -40.08510638, -9.978723404);
            Point3d TargetPointSum = new Point3d(1698, -1884, -469);
            Assert.IsTrue(sum == TargetPointSum);
            Assert.IsTrue(Point3d.NearlyEquals(average, TargetPointAverage, 1e-7));
        }

        [TestMethod]
        public void Higher()
        {
            Point3d Higher = _Points.Aggregate((acc, cur) => Point3d.Higher(acc, cur));
            Point3d TargetPoint = new Point3d(987,958,954);
            Assert.IsTrue(Higher == TargetPoint);
        }

        [TestMethod]
        public void Lower()
        {
            Point3d Lower = _Points.Aggregate((acc, cur) => Point3d.Lower(acc, cur));
            Point3d TargetPoint = new Point3d(-999, -1250, -990);
            Assert.IsTrue(Lower == TargetPoint);
        }

        [TestMethod]
        public void ClusterTest()
        {
            Point3d p1 = new Point3d(143, -353, 5678);
            Point3d p2 = new Point3d(142, -352, 5679);
            Point3d p3 = new Point3d(144, -351, 5677);

            Point3d q1 = new Point3d(-675, -456, 44);
            Point3d q2 = new Point3d(-672, -456, 45);
            Point3d q3 = new Point3d(-674, -455, 45);
            Point3d q4 = new Point3d(-675, -456, 44);


            Point3d r1 = new Point3d(-1786, 5453, 86);
            Point3d r2 = new Point3d(-1788, 5455, 88);
            Point3d r3 = new Point3d(-1785, 5454, 85);

            List<Point3d> Points = new List<Point3d>();
            Points.Add(p1);
            Points.Add(p2);
            Points.Add(p3);

            Points.Add(q1);
            Points.Add(q2);
            Points.Add(q3);
            Points.Add(q4);

            Points.Add(r1);
            Points.Add(r2);
            Points.Add(r3);

            Points.Shuffle();

            Point3dCluster Cluster = new Point3dCluster(10);
            foreach (Point3d p in Points)
            {
                Cluster.Add(p);
            }

            Assert.IsTrue(Cluster.Count == 3);
            


        



    }
    
    }
}
