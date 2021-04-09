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
            mockedNormal.Arrange(x => x.X).Returns(1);
            mockedNormal.Arrange(x => x.Y).Returns(0);
            mockedNormal.Arrange(x => x.Z).Returns(0);

            Point3d mockedPoint = _mockCreationHelper.Create<Point3d>();
            mockedPoint.Arrange(x => x.X).Returns(-56);
            mockedPoint.Arrange(x => x.Y).Returns(32);
            mockedPoint.Arrange(x => x.Z).Returns(122);

            double distance = 50;
            Projector.Projector p = new Projector.Projector(mockedPoint, distance,mockedNormal);


            Assert.AreEqual(p.V1.X, 0);
            Assert.AreEqual(p.V1.Y, 1);
            Assert.AreEqual(p.V1.Z, 0);

            Assert.AreEqual(p.V2.X, 1);
            Assert.AreEqual(p.V2.Y, 0);
            Assert.AreEqual(p.V2.Z, 0);

            _mockCreationHelper.AssertAll();

        }


    }
}
