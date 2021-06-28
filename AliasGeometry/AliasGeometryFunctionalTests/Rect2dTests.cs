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
    public class Rect2dTests
    {
        [TestMethod]
        public void DistanceToRect()
        {
            Point2d p1 = new Point2d(-4500, 5000);
            Point2d p2 = new Point2d(7000, -3000);

            Rectangle2d r = new Rectangle2d(p1, p2);
            Point2d p3 = new Point2d(-4490, 2000);
            Assert.IsTrue(r.IsPointInside(p3));
            double distance = r.PointDistance(p3);
            Assert.AreEqual(10, distance);
        }
    
    
    }
}
