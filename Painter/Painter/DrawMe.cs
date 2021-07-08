using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Painter
{
    public class DrawMe
    {
        public Pen Shaun { get; }
        public Brush Basil { get; }

        void ConvertToScreen(int width, int height) { }

    }

    public class Line : DrawMe
    {
        public Line(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public Point p1 { get; set; }
        public Point p2 { get; set; }
    }

    public class Ellipse : DrawMe
    {
        public Ellipse(Rectangle rtangle)
        {
            this.rtangle = rtangle;
        }

        public Rectangle rtangle { get; set; }

        


    
    
    }





}
