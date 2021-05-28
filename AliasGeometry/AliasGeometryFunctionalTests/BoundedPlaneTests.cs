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
        private BoundedPlane3d _boundedPlane3D;
        private Vector3d _N;
        private Point3d _tl;
        private Point3d _tr;
        private Point3d _br;
        private Point3d _bl;
     
        public BoundedPlaneTests()
        {
            _tl = new Point3d(-45, 456, 706);
            _tr = new Point3d(-380.7461108, 120.2538892, 862.6815184);
            _br = new Point3d(-546.931957, -45.93195696, 150.4564633);
            _bl = new Point3d(-211.1858462, 289.8141538, -6.225055062);
           _N = new Vector3d(0.707106781, -0.707106781, 0);
           _boundedPlane3D = new BoundedPlane3d(_N, _tl, _br, _tr, _bl);

        }
        
        [TestMethod]
        public void CenterPointTest()
        {
            Assert.IsTrue(_boundedPlane3D.IsValid(1e-8));
            Assert.IsTrue(_boundedPlane3D.P.X == -295.9659785);
            Assert.IsTrue(_boundedPlane3D.P.Y == 205.03402152);
            Assert.IsTrue(_boundedPlane3D.P.Z == 428.22823165);
        }

        [TestMethod]
        public void CartesianPointTest()
        {
          
            double w = _boundedPlane3D.Top.Length();
            double h = _boundedPlane3D.Right.Length();
            int iw = Convert.ToInt32(Math.Floor(w/2 + 0.5));
            int ih = Convert.ToInt32(Math.Floor(h/2 + 0.5));

            Assert.IsTrue(_boundedPlane3D.IsValid(1e-10));

            Point2d pcenter = _boundedPlane3D.CartiesianPoint(_boundedPlane3D.P);
            Assert.IsTrue(pcenter.X == 0);
            Assert.IsTrue(pcenter.Y == 0);

            Point2d ptl = _boundedPlane3D.CartiesianPoint(_tl);
            Assert.IsTrue(ptl.X == -iw);
            Assert.IsTrue(ptl.Y == ih);

            Point2d ptr = _boundedPlane3D.CartiesianPoint(_tr);
            Assert.IsTrue(ptr.X == iw);
            Assert.IsTrue(ptr.Y == ih);

            Point2d btl = _boundedPlane3D.CartiesianPoint(_bl);
            Assert.IsTrue(btl.X == -iw);
            Assert.IsTrue(btl.Y == -ih);

            Point2d btr = _boundedPlane3D.CartiesianPoint(_br);
            Assert.IsTrue(btr.X == iw);
            Assert.IsTrue(btl.Y == -ih);


        }

        [TestMethod]
        public void IsPointInPlane1()
        {
            Point3d p = new Point3d(-308.8998004, 192.1001996, 516.400974);
            Assert.IsTrue(_boundedPlane3D.IsPointOnPlane(p,1e-8));
            Plane3d basePlane = _boundedPlane3D;
            Assert.IsTrue(basePlane.IsPointOnPlane(p,1e-8));
        }

        [TestMethod]
        public void IsPointInPlane2()
        {
            Point3d p = new Point3d(-659.4187401, -158.4187401, 679.9764792); 
            Assert.IsFalse(_boundedPlane3D.IsPointOnPlane(p));
            Plane3d basePlane = _boundedPlane3D;
            Assert.IsTrue(basePlane.IsPointOnPlane(p,1e-8));
        }

        [TestMethod]
        public void IsPointInPlane3()
        {
            Point3d p = new Point3d(-308.8998004, 192.1001996, 700);
            Assert.IsFalse(_boundedPlane3D.IsPointOnPlane(p));
            Plane3d basePlane = _boundedPlane3D;
            Assert.IsFalse(basePlane.IsPointOnPlane(p));
        }



    }
}
