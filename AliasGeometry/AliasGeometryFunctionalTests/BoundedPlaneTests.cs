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
    public class BoundedPlaneTests
    {
        [TestMethod]
        public void CenterPointTest()
        {
            Point3d tl = new Point3d(-45, 456, 706);
            Point3d tr = new Point3d(-443.933662, 757.4165446, 706);
            Point3d br = new Point3d(-443.933662, 757.4165446, 1456);
            Point3d bl = new Point3d(-45, 456, 1456);
            Vector3d n = new Vector3d(0.602833089, 0.797867324, 0);
            BoundedPlane3d boundedPlane3d = new BoundedPlane3d(n, tl, br, tr, bl);
            Assert.IsTrue(boundedPlane3d.IsValid(1e-8));
            Assert.IsTrue(boundedPlane3d.P.X == -244.466831);
            Assert.IsTrue(boundedPlane3d.P.Y == 606.7082723);
            Assert.IsTrue(boundedPlane3d.P.Z == 1081);
        }

        [TestMethod]
        public void CartesianPointTest()
        {
            Point3d tl = new Point3d(-45, 456, 706);
            Point3d tr = new Point3d(-380.7461108, 120.2538892, 862.6815184);
            Point3d br = new Point3d(-546.931957, -45.93195696, 150.4564633);
            Point3d bl = new Point3d(-211.1858462, 289.8141538, -6.225055062);
            Vector3d n = new Vector3d(0.707106781, -0.707106781, 0);
            BoundedPlane3d boundedPlane3d = new BoundedPlane3d(n, tl, br, tr, bl);
            double w = boundedPlane3d.Top.Length();
            double h = boundedPlane3d.Right.Length();
            int iw = Convert.ToInt32(Math.Floor(w/2 + 0.5));
            int ih = Convert.ToInt32(Math.Floor(h/2 + 0.5));

            Assert.IsTrue(boundedPlane3d.IsValid(1e-10));

            Point2d pcenter = boundedPlane3d.CartiesianPoint(boundedPlane3d.P);
            Assert.IsTrue(pcenter.X == 0);
            Assert.IsTrue(pcenter.Y == 0);

            Point2d ptl = boundedPlane3d.CartiesianPoint(tl);
            Assert.IsTrue(ptl.X == -iw);
            Assert.IsTrue(ptl.Y == ih);

            Point2d ptr = boundedPlane3d.CartiesianPoint(tr);
            Assert.IsTrue(ptr.X == iw);
            Assert.IsTrue(ptr.Y == ih);

            Point2d btl = boundedPlane3d.CartiesianPoint(bl);
            Assert.IsTrue(btl.X == -iw);
            Assert.IsTrue(btl.Y == -ih);

            Point2d btr = boundedPlane3d.CartiesianPoint(br);
            Assert.IsTrue(btr.X == iw);
            Assert.IsTrue(btl.Y == -ih);


        }
    
    
    }
}
