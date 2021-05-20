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
    public class Line3dTest
    {
        [TestMethod]
        public void IntersectionTest()
        {
            Point3d p1 = new Point3d(5, 6, - 20);
            Point3d q1 = new Point3d(5, 6, 50);
            Line3d l1 = new Line3d(p1, q1);

            Point3d p2 = new Point3d(38.83126447, -30.81637604, 14);
            Point3d q2 = new Point3d(-25.44813802, 39.13473843, 14);
            Line3d l2 = new Line3d(p2, q2);

            Point3d I = new Point3d();
            bool interescts = Line3d.Intersection(l1, l2, ref I);
            Assert.IsTrue(interescts);
        }
    
    
    
    }
}
