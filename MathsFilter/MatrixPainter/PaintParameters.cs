using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MatrixPainter
{
    internal class PaintParameters
    {
        internal bool Stretch { get; set; }

        internal int StretchTop { get; set; }
        internal int StretchBottom { get; set; }

        internal string RedMatrix { get; set; }
        internal string GreenMatrix { get; set; }
        internal string BlueMatrix { get; set; }

        internal string AllMatrix { get; set; }

       

        internal PaintParameters()
        {
            Stretch = false;
            StretchTop = 256 * 256 * 256 - 1;
            StretchBottom = 0;
        }

        internal bool All()
        {
            return AllMatrix != null && File.Exists(AllMatrix) && RedMatrix == null && BlueMatrix == null && GreenMatrix == null;
          
        }

        internal bool RGB()
        {
            bool red = RedMatrix != null && File.Exists(RedMatrix);
            bool green = GreenMatrix != null && File.Exists(GreenMatrix);
            bool blue = BlueMatrix != null && File.Exists(BlueMatrix);
            return AllMatrix == null && (red || green || blue);
        }
        
    
    }
}
