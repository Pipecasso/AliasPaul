using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using AliasGeometry;

namespace Painter
{
    public class Canvas
    {
        private Bitmap _canvas;

        public Canvas(Bitmap bitty)
        {
            _canvas = bitty;
        }

        public Bitmap Bitmap { get => _canvas; }

        #region Transform
        private Point2d TransformToScreen(Point2d pin)
        {
            int ix = _canvas.Width / 2 + pin.X;
            int iy = _canvas.Height / 2 - pin.Y;
            return new Point2d(ix, iy);

        }

        private Line2d TransformToScreen(Line2d lin)
        {
            Point2d p1 = TransformToScreen(lin.start);
            Point2d p2 = TransformToScreen(lin.end);
            return new Line2d(p1, p2);
        }

        #endregion


    }
}
