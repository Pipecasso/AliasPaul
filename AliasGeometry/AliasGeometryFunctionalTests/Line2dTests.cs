using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AliasGeometry;

namespace AliasGeometryFunctionalTests
{
    [TestClass]
    public class Line2dTests
    {
        [TestMethod]
        public void Intersection()
        {
            Line2d l1 = new Line2d(new Point2d(-45,67), new Point2d(131,-7));
            Line2d l2 = new Line2d(new Point2d(-34,38), new Point2d(106,-24));
            Point2d intersection;
            bool intersect = Line2d.Intersection(l1, l2, out intersection);
            Assert.IsFalse(intersect);
            Assert.IsTrue(intersection.X == -1122);
            Assert.IsTrue(intersection.Y == 520);
        }

        [TestMethod] 
        public void DistanceToPoint()
        {
            Line2d l1 = new Line2d(new Point2d(-45, 67), new Point2d(131, -7));
            Point2d p = new Point2d(106, -24);
            double distance = l1.DistanceFromExtendedLine(p);
            Assert.IsTrue(Math.Abs(distance - 25.07987) < 1e-5);
        }
    }
}
