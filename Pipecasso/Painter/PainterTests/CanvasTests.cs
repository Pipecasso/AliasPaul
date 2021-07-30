using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using AliasGeometry;
using Painter;

namespace PainterTests
{
    [TestClass]
    public class CanvasTests
    {
        [TestMethod]
        public void PointToPoint()
        {
            Bitmap bitmap = new Bitmap(5000, 1500);
            //centre
            Point2d pcartesian = new Point2d(0, 0);
            Canvas canvas = new Canvas(bitmap);
            Point pscreen = canvas.PointToPoint(pcartesian);
            Assert.AreEqual(2500, pscreen.X);
            Assert.AreEqual(750, pscreen.Y);

            //top left
            Point2d pcarttl = new Point2d(-2500, 750);
            Point screentl = canvas.PointToPoint(pcarttl);
            Assert.AreEqual(0, screentl.X);
            Assert.AreEqual(0, screentl.Y);

            //top right 
            Point2d pcarttr = new Point2d(2500, 750);
            Point screentr = canvas.PointToPoint(pcarttr);
            Assert.AreEqual(5000, screentr.X);
            Assert.AreEqual(0, screentr.Y);

            //bottom left
            Point2d pcartbl = new Point2d(-2500, -750);
            Point screenbl = canvas.PointToPoint(pcartbl);
            Assert.AreEqual(0, screenbl.X);
            Assert.AreEqual(1500, screenbl.Y);

            //bottom right
            Point2d pcartbr = new Point2d(2500, -750);
            Point screenbr = canvas.PointToPoint(pcartbr);
            Assert.AreEqual(5000, screenbr.X);
            Assert.AreEqual(1500, screenbr.Y);
        }
    
    
    }
}
