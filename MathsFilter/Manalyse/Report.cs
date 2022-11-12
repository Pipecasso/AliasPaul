using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFilter;

namespace Manalyse
{
    public class Report
    {
        MatrixAnalysis _ma;
        
        public Report(MatrixAnalysis ma)
        {
            _ma = ma;
        }

        public void Analyse()
        {
            System.Console.WriteLine($"   Minimum {_ma.Minumum}");
            System.Console.WriteLine($"        Q1 { _ma.Q1}");
            System.Console.WriteLine($"    Median { _ma.Median}");
            System.Console.WriteLine($"       Q3  { _ma.Q3}");
            System.Console.WriteLine($"  Maximum  {_ma.Maximum}");
            System.Console.WriteLine($"     Mean  {_ma.Mean}");
            System.Console.WriteLine($"ValueCount {_ma.ValCount}");

            double Area = Convert.ToDouble(_ma.Area);
            double negative = Convert.ToDouble(_ma.Negative) * 100 / Area;
            double mono = Convert.ToDouble(_ma.Mono) * 100 / Area;
            double duo = Convert.ToDouble(_ma.Dou) * 100 / Area;
            double rgb = Convert.ToDouble(_ma.RGB) * 100 / Area;
            double toohigh = Convert.ToDouble(_ma.OutOfRange) * 100 / Area;
            double oos = Convert.ToDouble(_ma.OutOfRange2) * 100 / Area;
            

            System.Console.WriteLine();
            System.Console.WriteLine($"Negative      {negative}");
            System.Console.WriteLine($"    Mono      {mono}");
            System.Console.WriteLine($"    Duo       {duo}");
            System.Console.WriteLine($"    Rgb       {rgb}");
            System.Console.WriteLine($"Too High      {toohigh}");
            System.Console.WriteLine($"Off The Scale {oos}");
        }

    }
}
