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
    public class PlaneTests
    {
        private Plane3d _plane;
        

        public PlaneTests()
        {
            Point3d p = new Point3d(-450,400,742);
            Vector3d n = new Vector3d(1, -1, 0);
            _plane = new Plane3d(n, p);
        }
        [TestMethod]
        public void PointInPlane()
        {
            Point3d p = new Point3d(-410.4020203, 439.5979797, 787);
            Assert.IsTrue(_plane.IsPointOnPlane(p));
        }

        [TestMethod]
        public void PointOutOfPlane()
        {
            Point3d p = new Point3d(-498.0832611, 527.2792206, 787);
            Assert.IsFalse(_plane.IsPointOnPlane(p));
            Point3d i = _plane.NearestPoint(p);
            Point3d ptest = new Point3d(-410.4020203, 439.5979797, 787);
            Assert.IsTrue(i == ptest);

        }


    }
}
