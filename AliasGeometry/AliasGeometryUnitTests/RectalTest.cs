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

        private Rectangle2d RectalMake()
        {
            Point2d mockedPointa = _mockCreationHelper.Create<Point2d>();
            Point2d mockedPointb = _mockCreationHelper.Create<AliasGeometry.Point2d>();
      

            mockedPointa.Arrange(x => x.X).Returns(-100);
            mockedPointa.Arrange(x => x.Y).Returns(500);

            mockedPointb.Arrange(x => x.X).Returns(700);
            mockedPointb.Arrange(x => x.Y).Returns(-800);

         

            AliasGeometry.Rectangle2d rectangle2d = new AliasGeometry.Rectangle2d(mockedPointa, mockedPointb);
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
            mockedPointq.Arrange(x => x.Y).Returns(-400);

            Rectangle2d rectangle2d = RectalMake();
            Assert.IsTrue(rectangle2d.IsPointInside(mockedPointp));
            Assert.IsFalse(rectangle2d.IsPointInside(mockedPointq));

            _mockCreationHelper.AssertAll();
        }

        /*   public void LineInside()
           {
               Point2d intersection;
               Rectangle2d rectangle2d = RectalMake();
               Line2d mockLine = _mockCreationHelper.Create<Line2d>();

               Point2d mockedPointp = _mockCreationHelper.Create<AliasGeometry.Point2d>();
               mockedPointp.Arrange(x => x.X).Returns(-50);
               mockedPointp.Arrange(x => x.Y).Returns(-100);

               Point2d mockedPointq = _mockCreationHelper.Create<AliasGeometry.Point2d>();
               mockedPointq.Arrange(x => x.X).Returns(600);
               mockedPointq.Arrange(x => x.Y).Returns(-700);

               mockLine.Arrange(x => x.start).Returns(mockedPointp);
               mockLine.Arrange(x => x.end).Returns(mockedPointq);

               RectalProbeLineResult rectalProbeLineResult = rectangle2d.BoundaryIntersection(mockLine, out intersection);
               Assert.IsTrue(rectalProbeLineResult == RectalProbeLineResult.Contained);

               _mockCreationHelper.AssertAll();
           }

           public void LineOutside()
           {
               Point2d intersection;
               Rectangle2d rectangle2d = RectalMake();
               Line2d mockLine = _mockCreationHelper.Create<Line2d>();
               RectalProbeLineResult rectalProbeLineResult = rectangle2d.BoundaryIntersection(mockLine, out intersection);
               Assert.IsTrue(rectalProbeLineResult == RectalProbeLineResult.Outside);
               _mockCreationHelper.AssertAll();
           }*/

        [TestMethod]
        public void LineIntersect()
        {
            Point2d intersection;
            Rectangle2d rectangle2d = RectalMake();

            Point2d mockedPointp = _mockCreationHelper.Create<AliasGeometry.Point2d>();
            mockedPointp.Arrange(x => x.X).Returns(-50);
            mockedPointp.Arrange(x => x.Y).Returns(-100);

            Point2d mockedPointq = _mockCreationHelper.Create<AliasGeometry.Point2d>();
            mockedPointq.Arrange(x => x.X).Returns(600);
            mockedPointq.Arrange(x => x.Y).Returns(-700);

            Line2d l = _mockCreationHelper.Create<Line2d>();
            l.Arrange(x => x.start).Returns(mockedPointp);
            l.Arrange(x => x.end).Returns(mockedPointq);



            Point2d i = null;
            _mockCreationHelper.ArrangeStatic<bool>(() => Line2d.Intersection(l, rectangle2d.Left, out i)).Returns(false);
            _mockCreationHelper.ArrangeStatic<bool>(() => Line2d.Intersection(l, rectangle2d.Right, out i)).Returns(false);


            RectalProbeLineResult rectalProbeLineResult = rectangle2d.BoundaryIntersection(l, out intersection);
            Assert.IsTrue(rectalProbeLineResult == RectalProbeLineResult.Boundary);
            Assert.IsTrue(intersection.X == 31.25);
            Assert.IsTrue(intersection.Y == -800);
            _mockCreationHelper.AssertAll();
        }

        public void LineVertical()
        {
            Point2d intersection;
            Rectangle2d rectangle2d = RectalMake();
            Line2d mockLine = _mockCreationHelper.Create<Line2d>();
            RectalProbeLineResult rectalProbeLineResult = rectangle2d.BoundaryIntersection(mockLine, out intersection);
            Assert.IsTrue(rectalProbeLineResult == RectalProbeLineResult.Boundary);
            _mockCreationHelper.AssertAll();
        }
    }
}
