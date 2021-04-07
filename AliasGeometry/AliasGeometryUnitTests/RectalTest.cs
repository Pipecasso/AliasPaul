using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mocka;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
using AliasGeometry;

namespace AliasGeometryUnitTests
{
    [TestClass]
    public class RectalTest
    {
        private MockCreationHelper _mockCreationHelper;
        private Point2d _mockedPointa;
        private Point2d _mockedPointb;

        private Rectangle2d RectalMake()
        {
            _mockedPointa = _mockCreationHelper.Create<Point2d>();
            _mockedPointb = _mockCreationHelper.Create<AliasGeometry.Point2d>();
      
            _mockedPointa.Arrange(x => x.X).Returns(-100);
            _mockedPointa.Arrange(x => x.Y).Returns(500);

            _mockedPointb.Arrange(x => x.X).Returns(700);
            _mockedPointb.Arrange(x => x.Y).Returns(-800);



            AliasGeometry.Rectangle2d rectangle2d = new AliasGeometry.Rectangle2d(_mockedPointa, _mockedPointb);
            return rectangle2d;
        }

        public RectalTest()
        {
            _mockCreationHelper = new MockCreationHelper();
        }

        [TestMethod]
        public void InsideOutside()
        {

            Point2d mockedPointp = _mockCreationHelper.Create<AliasGeometry.Point2d>();
            Point2d mockedPointq = _mockCreationHelper.Create<AliasGeometry.Point2d>();

            mockedPointp.Arrange(x => x.X).Returns(600);
            mockedPointp.Arrange(x => x.Y).Returns(-750);

            mockedPointq.Arrange(x => x.X).Returns(-300);
           // mockedPointq.Arrange(x => x.Y).Returns(-400);

            Rectangle2d rectangle2d = RectalMake();
            Assert.IsTrue(rectangle2d.IsPointInside(mockedPointp));
            Assert.IsFalse(rectangle2d.IsPointInside(mockedPointq));

            _mockCreationHelper.AssertAll();
        }


        [TestMethod]
        public void LineIntersect()
        {
            Point2d intersection;
            Rectangle2d rectangle2d = RectalMake();
            Line2d mockedLine = _mockCreationHelper.Create<Line2d>();
     
            Point2d intersectionLeft = null; 
            Point2d intersectionRight = null;
            Point2d intersectionTop = null;
            Point2d intersectionBottom = new Point2d(31, -800);

            _mockCreationHelper.ArrangeStatic<bool>(() => Line2d.Intersection(mockedLine, Arg.Matches<Line2d>(x=>object.ReferenceEquals(x.start,rectangle2d.a) && object.ReferenceEquals(x.end,rectangle2d.c)), out intersectionLeft)).Returns(false);
            _mockCreationHelper.ArrangeStatic<bool>(() => Line2d.Intersection(mockedLine, Arg.Matches<Line2d>(x => object.ReferenceEquals(x.start, rectangle2d.b) && object.ReferenceEquals(x.end, rectangle2d.d)), out intersectionRight)).Returns(false);
            _mockCreationHelper.ArrangeStatic<bool>(() => Line2d.Intersection(mockedLine, Arg.Matches<Line2d>(x => object.ReferenceEquals(x.start, rectangle2d.a) && object.ReferenceEquals(x.end, rectangle2d.b)), out intersectionTop)).Returns(false);
            _mockCreationHelper.ArrangeStatic<bool>(() => Line2d.Intersection(mockedLine, Arg.Matches<Line2d>(x => object.ReferenceEquals(x.start, rectangle2d.c) && object.ReferenceEquals(x.end, rectangle2d.d)), out intersectionBottom)).Returns(true);

            RectalProbeLineResult rectalProbeLineResult = rectangle2d.BoundaryIntersection(mockedLine, out intersection);
            Assert.IsTrue(rectalProbeLineResult == RectalProbeLineResult.Boundary);
            Assert.IsTrue(intersection.X == 31);
            Assert.IsTrue(intersection.Y == -800);
            _mockCreationHelper.AssertAll();
        }
    }
}
