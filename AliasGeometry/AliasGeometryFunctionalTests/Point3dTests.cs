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
    public class Point3dTests
    {
        [TestMethod]
        public void AveragePoint()
        {
            List<Point3d> points = ALotOfPoints.GetSomePoints();
            Point3d sum = points.Aggregate((acc, cur) => acc + cur);
            double count = Convert.ToDouble(points.Count);
            Point3d average = points.Aggregate((acc, cur) => (acc + cur) / count);

            //Point3d TargetPoint = new Point3d(36.12765957, -40.08510638, -9.978723404);
            Point3d TargetPoint = new Point3d(1698, -1884, -469);
            Assert.IsTrue(sum == TargetPoint);

        }
    
    }
}
