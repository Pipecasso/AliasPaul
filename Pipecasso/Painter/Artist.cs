using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projector;
using System.Drawing;

namespace Painter
{
    public class Artist
    {
        private Dictionary<dynamic, Shapes2d> _PaintThis;
        public Artist(Dictionary<dynamic,Shapes2d> paintthis)
        {
            _PaintThis = paintthis;
        }

        public void PaintIt(Canvas canvas,Dictionary<dynamic,Func<dynamic,Tuple<Pen,Brush> >> tools,Dictionary<dynamic,Action<dynamic,Tuple<Pen,Brush>,Shapes2d>> instructions)
        {
            foreach (KeyValuePair<dynamic,Shapes2d> kvp in _PaintThis)
            {
                dynamic key = kvp.Key;
                Shapes2d shapes = kvp.Value;

                if (tools.ContainsKey(key) && instructions.ContainsKey(key))
                {
                    Tuple<Pen, Brush> tool = tools[key];
                    Action<dynamic, Tuple<Pen, Brush>, Shapes2d> instruction = instructions[key];
                    instruction(key, tool, shapes);
                }
            }
        }
    }
}
