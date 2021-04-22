using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JointComparer;

namespace JointAnalyser
{
    class Program
    {
        static void Main(string[] args)
        {
            string folder = args[0];
            JointCompare jc = new JointCompare();
            jc.Light(folder);

            
        }
    }
}
