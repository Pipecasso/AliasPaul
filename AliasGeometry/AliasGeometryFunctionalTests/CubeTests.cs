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
    public class CubeTests
    {
        private List<Point3d> _Points;
        private CubeView _CubeTopLeft;
        private CubeView _CubeTopRight;
        private CubeView _CubeBottomLeft;
        private CubeView _CubeBottomRight;
        private Vertex _ftl;
        private Vertex _fbr;
        private Vertex _btl;
        private Vertex _ftr;
        private Vertex _bbl;
        private Vertex _fbl;
        private Vertex _bbr;
        private Vertex _btr;
        private const double _lowx = -999;
        private const double _highx = 987;
        private const double _lowy = -1250;
        private const double _highy = 958;
        private const double _lowz = -990;
        private const double _highz = 954;



        public CubeTests()
        {
            _Points = ALotOfPoints.GetSomePoints();
           
            _CubeTopLeft = new CubeView(_Points, NorthArrow.TopLeft);
            _CubeTopRight = new CubeView(_Points, NorthArrow.TopRight);
            _CubeBottomLeft = new CubeView(_Points, NorthArrow.BottomLeft);
            _CubeBottomRight = new CubeView(_Points, NorthArrow.BottomRight);

            _ftl = new Vertex(Face.Front, Face.Top, Face.Left);
            _fbr = new Vertex(Face.Front, Face.Bottom, Face.Right);
            _btl = new Vertex(Face.Back, Face.Top, Face.Left);
            _ftr = new Vertex(Face.Front, Face.Top, Face.Right);
            _bbl = new Vertex(Face.Back, Face.Bottom, Face.Left);
            _fbl = new Vertex(Face.Front, Face.Bottom, Face.Left);
            _bbr = new Vertex(Face.Back, Face.Bottom, Face.Right);
            _btr = new Vertex(Face.Back, Face.Top, Face.Right);
        }



        [TestMethod]
        public void CubeCentre()
        {

            Point3d Low = new Point3d(_lowx, _lowy, _lowz);
            Point3d High = new Point3d(_highx, _highy, _highz);
            Point3d centerwant = Point3d.MidPoint(Low, High);

            Assert.AreEqual(centerwant.X, _CubeTopLeft.Center.X);
            Assert.AreEqual(centerwant.Y, _CubeTopLeft.Center.Y);
            Assert.AreEqual(centerwant.Z, _CubeTopLeft.Center.Z);
            Assert.AreEqual(centerwant.X, _CubeTopRight.Center.X);
            Assert.AreEqual(centerwant.Y, _CubeTopRight.Center.Y);
            Assert.AreEqual(centerwant.Z, _CubeTopRight.Center.Z);
        }

        [TestMethod]
        public void Distances()
        {
            Assert.AreEqual(2208, _CubeTopLeft.LeftRightDistance());
            Assert.AreEqual(1986, _CubeTopLeft.FrontBackDistance());
            Assert.AreEqual(1944, _CubeTopLeft.TopBottomDistance());
            Assert.AreEqual(1986, _CubeTopRight.LeftRightDistance());
            Assert.AreEqual(2208, _CubeTopRight.FrontBackDistance());
            Assert.AreEqual(1944, _CubeTopRight.TopBottomDistance());
        }


        private void VertexVectorsTest(CubeView cv, Vertex v)
        {
            Vertex vlr = Vertex.OppositeLR(v);
            Vertex vud = Vertex.OppositeUD(v);
            Vertex vfb = Vertex.OppositeFB(v);

            Vector3d lr = new Vector3d(cv[v], cv[vlr]);
            Vector3d ud = new Vector3d(cv[v], cv[vud]);
            Vector3d fb = new Vector3d(cv[v], cv[vfb]);
            lr.Normalise();
            ud.Normalise();
            fb.Normalise();

            Assert.IsTrue(lr.Orthogonal());
            Assert.IsTrue(ud.Orthogonal());
            Assert.IsTrue(fb.Orthogonal());

            Assert.AreEqual(0, Vector3d.Dot(lr, ud));
            Assert.AreEqual(0, Vector3d.Dot(lr, fb));
            Assert.AreEqual(0, Vector3d.Dot(fb, ud));


        }



        [TestMethod]
        public void Orthogonal()
        {
            VertexVectorsTest(_CubeTopLeft, _ftl);
            VertexVectorsTest(_CubeTopLeft, _bbr);
            VertexVectorsTest(_CubeTopRight, _ftl);
            VertexVectorsTest(_CubeTopRight, _bbr);
            VertexVectorsTest(_CubeBottomRight, _ftl);
            VertexVectorsTest(_CubeBottomRight, _bbr);
            VertexVectorsTest(_CubeBottomLeft, _ftl);
            VertexVectorsTest(_CubeBottomLeft, _bbr);
        }

        [TestMethod]
        public void VertexCoordsNorthTL()
        {

            Point3d p = _CubeTopLeft[_fbr];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeTopLeft[_btl];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_highz, p.Z);
            p = _CubeTopLeft[_ftr];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_highz, p.Z);
            p = _CubeTopLeft[_bbl];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeTopLeft[_fbl];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeTopLeft[_ftl];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_highz, p.Z);
            p = _CubeTopLeft[_bbr];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeTopLeft[_btr];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_highz, p.Z);



        }

        [TestMethod]
        public void VertexCoordsNorthTR()
        {

            Point3d p = _CubeTopRight[_fbr];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeTopRight[_btl];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_highz, p.Z);
            p = _CubeTopRight[_ftr];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_highz, p.Z);
            p = _CubeTopRight[_bbl];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeTopRight[_fbl];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeTopRight[_ftl];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_highz, p.Z);
            p = _CubeTopRight[_bbr];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeTopRight[_btr];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_highz, p.Z);
        }

        [TestMethod]
        public void VertexCoordsNorthBR()
        {
            Point3d p = _CubeBottomRight[_fbr];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeBottomRight[_btl];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_highz, p.Z);
            p = _CubeBottomRight[_ftr];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_highz, p.Z);
            p = _CubeBottomRight[_bbl];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeBottomRight[_fbl];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeBottomRight[_ftl];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_highz, p.Z);
            p = _CubeBottomRight[_bbr];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeBottomRight[_btr];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_highz, p.Z);
        }

        [TestMethod]
        public void VertexCoordsNorthBL()
        {
            Point3d p = _CubeBottomLeft[_fbr];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeBottomLeft[_btl];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_highz, p.Z);
            p = _CubeBottomLeft[_ftr];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_highz, p.Z);
            p = _CubeBottomLeft[_bbl];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeBottomLeft[_fbl];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeBottomLeft[_ftl];
            Assert.AreEqual(_highx, p.X);
            Assert.AreEqual(_highy, p.Y);
            Assert.AreEqual(_highz, p.Z);
            p = _CubeBottomLeft[_bbr];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_lowz, p.Z);
            p = _CubeBottomLeft[_btr];
            Assert.AreEqual(_lowx, p.X);
            Assert.AreEqual(_lowy, p.Y);
            Assert.AreEqual(_highz, p.Z);
        }


        void TestCubePlanes(CubeView cv)
        {
            foreach (KeyValuePair<Face, BoundedPlane3d> kvp in cv.SixPlanes())
            {
                BoundedPlane3d plane = kvp.Value;
                Assert.IsTrue(plane.IsValid());
            }
        }

        void TestCubePlaneRelations(CubeView cv)
        {
            Dictionary<Face, BoundedPlane3d> sixPlanes = cv.SixPlanes();
            BoundedPlane3d front = sixPlanes[Face.Front];
            BoundedPlane3d back = sixPlanes[Face.Back];
            BoundedPlane3d top = sixPlanes[Face.Top];
            BoundedPlane3d bottom = sixPlanes[Face.Bottom];
            BoundedPlane3d left = sixPlanes[Face.Left];
            BoundedPlane3d right = sixPlanes[Face.Right];

            Assert.IsTrue(Vector3d.Parallel(front.N, back.N));
            Assert.IsTrue(Vector3d.Parallel(top.N, bottom.N));
            Assert.IsTrue(Vector3d.Parallel(left.N, right.N));

            Assert.IsTrue(Math.Abs(Vector3d.Dot(front.N, top.N)) <= double.Epsilon);
            Assert.IsTrue(Math.Abs(Vector3d.Dot(front.N, right.N)) <= double.Epsilon);
            Assert.IsTrue(Math.Abs(Vector3d.Dot(right.N, top.N)) <= double.Epsilon);
        }

        void TestCubePlaneCenters(CubeView cv)
        {
            Dictionary<Face, BoundedPlane3d> sixPlanes = cv.SixPlanes();
            BoundedPlane3d front = sixPlanes[Face.Front];
            BoundedPlane3d back = sixPlanes[Face.Back];
            BoundedPlane3d top = sixPlanes[Face.Top];
            BoundedPlane3d bottom = sixPlanes[Face.Bottom];
            BoundedPlane3d left = sixPlanes[Face.Left];
            BoundedPlane3d right = sixPlanes[Face.Right];

            Line3d frontback = new Line3d(front.P, back.P);
            Line3d leftright = new Line3d(left.P, right.P);
            Line3d bottomtop = new Line3d(bottom.P, top.P);

            Assert.IsTrue(frontback.Length() == cv.FrontBackDistance());
            Assert.IsTrue(leftright.Length() == cv.LeftRightDistance());
            Assert.IsTrue(bottomtop.Length() == cv.TopBottomDistance());

            double error1 = Math.Abs(Vector3d.Dot(frontback.Vector(true), leftright.Vector(true)));
            double error2 = Math.Abs(Vector3d.Dot(frontback.Vector(true), bottomtop.Vector(true)));
            double error3 = Math.Abs(Vector3d.Dot(bottomtop.Vector(true), leftright.Vector(true)));

            Assert.IsTrue(error1 <= double.Epsilon);
            Assert.IsTrue(error2 <= double.Epsilon);
            Assert.IsTrue(error3 <= double.Epsilon);
        }


        [TestMethod]
        public void CubePlanesValid()
        {
            TestCubePlanes(_CubeTopLeft);
            TestCubePlanes(_CubeTopRight);
            TestCubePlanes(_CubeBottomLeft);
            TestCubePlanes(_CubeBottomLeft);
        }

        [TestMethod]
        public void CubePlaneRelations()
        {
            TestCubePlaneRelations(_CubeTopLeft);
            TestCubePlaneRelations(_CubeTopRight);
            TestCubePlaneRelations(_CubeBottomLeft);
            TestCubePlaneRelations(_CubeBottomRight);
        }

        [TestMethod]
        public void CubePlaneCenters()
        {
            TestCubePlaneCenters(_CubeTopLeft);
            TestCubePlaneCenters(_CubeTopRight);
            TestCubePlaneCenters(_CubeBottomLeft);
            TestCubePlaneCenters(_CubeBottomRight);
        }


        [TestMethod]
        public void IntersectionTestPassThroguh()
        {
            Point3d p1 = _CubeTopLeft.FrontTopLeft;
            Point3d p2 = _CubeTopLeft.FrontTopRight;
            Point3d p3 = _CubeTopLeft.BackBottomLeft;
            Point3d p4 = _CubeTopLeft.BackBottomRight;
            Point3d m = Point3d.MidPoint(p1, p2);
            Point3d n = Point3d.MidPoint(p3, p4);
            Vector3d v1 = Vector3d.Normalise(new Vector3d(m, _CubeTopLeft.Center));
            double d = v1.Magnitude();
            Point3d p = _CubeTopLeft.Center + v1 * (d * 3);


            LineCubeIntersection le = _CubeTopLeft.Intersection(p, v1);
            Assert.IsTrue(le.intersection == LineCubeIntersection.Intersection.PassThrough);
            Assert.IsTrue(le.PointMap.Count == 2);
            Assert.IsTrue(le.FaceMap.Count == 4);
            Line3d interline = le.IntersectionLine();
            Assert.IsTrue((interline.P == m && interline.Q == n) || (interline.P == n && interline.Q == m));

        }

        [TestMethod]
        public void IntersectionTestPassThroguh2()
        {
            Point3d p1 = _CubeTopLeft.FrontBottomLeft;
            Point3d p2 = _CubeTopLeft.BackTopRight;

            Vector3d v1 = new Vector3d(p1, p2);
            double l = v1.Magnitude();
            v1.Normalise();

            Point3d p = p2 - v1 * (l * 3.7);

            LineCubeIntersection le = _CubeTopLeft.Intersection(p, v1);
            Assert.IsTrue(le.intersection == LineCubeIntersection.Intersection.PassThrough);
            Assert.IsTrue(le.PointMap.Count == 2);
            Assert.IsTrue(le.FaceMap.Count == 6);
            Line3d interline = le.IntersectionLine();
            Line3d linetarget = new Line3d(p1, p2);
            Assert.IsTrue(linetarget == interline);

        }

        [TestMethod]
        public void IntersectionTestSurface1()
        {
            Point3d p1 = _CubeTopLeft.FrontTopLeft;
            Point3d p2 = _CubeTopLeft.FrontTopRight;
            Point3d p3 = _CubeTopLeft.FrontBottomLeft;
            Point3d p4 = _CubeTopLeft.FrontBottomRight;
            Point3d m = Point3d.MidPoint(p1, p2);
            Point3d n = Point3d.MidPoint(p3, p4);
            Vector3d updown = new Vector3d(p1, p3);
            updown.Normalise();
            Point3d n1 = n + updown * 467;
            LineCubeIntersection le = _CubeTopLeft.Intersection(n1, (updown  * -1.0));
            Assert.IsTrue(le.intersection == LineCubeIntersection.Intersection.Surface);
            Assert.IsTrue(le.PointMap.Count == 2);
            Assert.IsTrue(le.FaceMap.Count == 2);
            Assert.IsTrue(le.SurfaceFaces.Count == 1);
            Assert.IsTrue(le.SurfaceFaces[0] == Face.Front);
            Line3d line3d = le.IntersectionLine();
            Line3d targetline = new Line3d(n, m);
            Assert.IsTrue(line3d == targetline);


        }
    }
}
