using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMaker
{
    internal class Parameters
    {

        private List<string> _messages;

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
            _messages = new List<string>();
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

        internal List<string> Messages => _messages;

        internal void AddMessage(string mess) { _messages.Add(mess); }
       
    }
}
