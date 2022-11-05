using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMaker
{
    internal class Parameters
    {
        
        internal Parameters()
        {
            Dimension = 1000;
            Scale = 1;
            XOffset = 0;
            YOffset = 0;
            a = 1;
            b = 1;
            picturename = string.Empty;
            FunctionName = string.Empty;
            power = 1;
        }
        internal int Dimension { get; set; }    
        internal double Scale { get; set; }
        internal int XOffset { get; set; }
        internal int YOffset { get; set; }
        internal string FunctionName { get; set; }

        internal double a { get; set; }
        internal double b { get; set; } 

        internal string picturename { get; set; }
        internal string matrixpath { get; set; }

        internal uint power { get; set; }
       
    }
}
