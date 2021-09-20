using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedPodLoader;
using Painter;
using Projector;
using System.Drawing;
using Intergraph.PersonalISOGEN;
using AliasPOD;
using AliasGeometry;

namespace PODPainter
{
    public class PODArtist : Artist
    {
        private POD _pod;
        private PODCanvas _podCanvas;

        public PODArtist(AliasPOD.POD pod,PODCanvas podCanvas, Dictionary<dynamic, Shapes2d> paintthis) : base(paintthis)
        { 
            _pod = pod;
            _podCanvas = podCanvas;
        }

        public void DrawIt(Dictionary<dynamic, Tuple<Pen, Brush>> pencilcase)
        {
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(_podCanvas.Bitmap);
            foreach (KeyValuePair<dynamic,Shapes2d> keyValuePair in _PaintThis)
            {
                Shapes2d shapestodraw = keyValuePair.Value;
                foreach (Line2d line2 in shapestodraw.Lines)
                {
                    Tuple<Point, Point> bitmappoints = _podCanvas.LineToLine(line2);
                    if (pencilcase.ContainsKey(keyValuePair.Key))
                    {
                        Tuple<Pen, Brush> penbrush = pencilcase[keyValuePair.Key];
                        g.DrawLine(penbrush.Item1,bitmappoints.Item1, bitmappoints.Item2);
                    }
                }  

                foreach (Cone2d cone in shapestodraw.Cones)
                {
                    _
                }
            }
        }

        public void SaveIt(string path)
        {
            _podCanvas.Bitmap.Save(path);
        }
    }
}
