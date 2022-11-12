using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFilter;
using Manalyse;

namespace MatrixReport
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TransformMatrix tm = new TransformMatrix();
            tm.Load(args[0]);
            MatrixAnalysis ma = new MatrixAnalysis(tm);
            Report r = new Report(ma);
            r.Analyse();
        
        }
    }
}
