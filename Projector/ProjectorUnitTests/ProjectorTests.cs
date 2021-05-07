using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Projector;
using AliasGeometry;
using Telerik.JustMock;
using Mocka;
using Telerik.JustMock.Helpers;

namespace ProjectorUnitTests
{
    [TestClass]
    public class ProjectorTests
    {
        private MockCreationHelper _mockCreationHelper;

        public ProjectorTests()
        {
            _mockCreationHelper = new MockCreationHelper();
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Vector3d mockedNormal = _mockCreationHelper.Create<Vector3d>();
            _mockCreationHelper.ArrangeStatic(() => Vector3d.Dot(mockedNormal, Arg.Matches<Vector3d>(x => x.X == 0 && x.Y == 0 && x.Z == 1))).Returns(0);
            mockedNormal.Arrange(x => x.X).Returns(0);
            mockedNormal.Arrange(x => x.Y).Returns(0);
            mockedNormal.Arrange(x => x.Z).Returns(1);

            Point3d mockedPoint = _mockCreationHelper.Create<Point3d>();
            mockedPoint.Arrange(x => x.X).Returns(-56);
            mockedPoint.Arrange(x => x.Y).Returns(32);
            mockedPoint.Arrange(x => x.Z).Returns(122);

            //only have to mock point and normal because I don't know how to mock an operator.

            double distance = 50;
            Camera camera = new Camera(mockedPoint, distance,mockedNormal);


            Assert.AreEqual(camera.V1.X, 0);
            Assert.AreEqual(camera.V1.Y, 1);
            Assert.AreEqual(camera.V1.Z, 0);

            Assert.AreEqual(camera.V2.X, 1);
            Assert.AreEqual(camera.V2.Y, 0);
            Assert.AreEqual(camera.V2.Z, 0);

            _mockCreationHelper.AssertAll();

        }

        [TestMethod]
        public void ConstructorTest2()
        {
            Vector3d mockedNormal = _mockCreationHelper.Create<Vector3d>();
            mockedNormal.Arrange(x => x.X).Returns(6 / Math.Sqrt(85));
            mockedNormal.Arrange(x => x.Y).Returns(-7 / Math.Sqrt(85));
            mockedNormal.Arrange(x => x.Z).Returns(0);

            Vector3d vCross2 = new Vector3d(-0.759256602, -0.650791373, 0);
            Vector3d vCross1 = new Vector3d(0, 0, -1);
            Vector3d vCross2Normalised = new Vector3d(-0.759256602, -0.650791373, 0);
            Vector3d vCross1Normalised = new Vector3d(0, 0, -1);

            Point3d mockedPoint = _mockCreationHelper.Create<Point3d>();
            mockedPoint.Arrange(x => x.X).Returns(-56);
            mockedPoint.Arrange(x => x.Y).Returns(32);
            mockedPoint.Arrange(x => x.Z).Returns(122);

            //only have to mock point and normal because I don't know how to mock an operator.

            _mockCreationHelper.ArrangeStatic(() => Vector3d.Dot(mockedNormal, Arg.Matches<Vector3d>(x => x.X == 0 && x.Y == 0 && x.Z == 1))).Returns(1);
            _mockCreationHelper.ArrangeStatic(() => Vector3d.CrossProduct(mockedNormal, Arg.Matches<Vector3d>(x => x.X == 0 && x.Y == 0 && x.Z == 1))).Returns(vCross2);
            _mockCreationHelper.ArrangeStatic(() => Vector3d.CrossProduct(vCross2Normalised, mockedNormal)).Returns(vCross1);

            _mockCreationHelper.ArrangeStatic(() => Vector3d.Normalise(vCross1)).Returns(vCross1Normalised);
            _mockCreationHelper.ArrangeStatic(() => Vector3d.Normalise(vCross2)).Returns(vCross2Normalised);

            double distance = 50;

            Camera camera = new Camera(mockedPoint, distance, mockedNormal);

            Assert.AreSame(vCross1Normalised, camera.V1);
            Assert.AreSame(vCross2Normalised, camera.V2);
            _mockCreationHelper.AssertAll();

        }

        [TestMethod]
        public void ProjectionTest()
        {
    
            //Vector3d vNormal = new Vector3d(6, -7, -2);
            //vNormal.Normalise();
            //Point3d point = new Point3d(-56, 32, 122);

            Vector3d mockedNormal = _mockCreationHelper.Create<Vector3d>();
            mockedNormal.Arrange(x => x.X).Returns(0.635998728);
            mockedNormal.Arrange(x => x.Y).Returns(-0.741998516);
            mockedNormal.Arrange(x => x.Z).Returns(-0.211999576);

            Point3d mockedcampoint = _mockCreationHelper.Create<Point3d>();
            mockedcampoint.Arrange(x => x.X).Returns(-56);
            mockedcampoint.Arrange(x => x.Y).Returns(32);
            mockedcampoint.Arrange(x => x.Z).Returns(122);


            Vector3d mockedV1 = _mockCreationHelper.Create<Vector3d>();
            mockedV1.Arrange(x => x.X).Returns(0.134831461);
            mockedV1.Arrange(x => x.Y).Returns(-0.157303371);
            mockedV1.Arrange(x => x.Z).Returns(0.95505618);

            Vector3d mockedV2 = _mockCreationHelper.Create<Vector3d>();
            mockedV2.Arrange(x => x.X).Returns(-0.741998516);
            mockedV2.Arrange(x => x.Y).Returns(-0.635998728);
            mockedV2.Arrange(x => x.Z).Returns(0);

            Point3d mockedN = _mockCreationHelper.Create<Point3d>();
            mockedN.Arrange(x => x.X).Returns(-24.2000636);
            mockedN.Arrange(x => x.Y).Returns(-5.0999258);
            mockedN.Arrange(x => x.Z).Returns(111.4000212);

            Point3d mockedPointToProject = _mockCreationHelper.Create<Point3d>();
            mockedPointToProject.Arrange(x => x.X).Returns(400);
            mockedPointToProject.Arrange(x => x.Y).Returns(-100);
            mockedPointToProject.Arrange(x => x.Z).Returns(23);

            Camera camera = new Camera();
            camera.Normal = mockedNormal;
            camera.V1 = mockedV1;
            camera.V2 = mockedV2;
            camera.N = mockedN;
            camera.distance = 50;
            camera.CameraPoint = mockedcampoint;


            Point2d projectedpoint = camera.ProjectPoint(mockedPointToProject);

            Assert.IsTrue(projectedpoint.X == -31);
            Assert.IsTrue(projectedpoint.Y == -2);
        }

    }
}
