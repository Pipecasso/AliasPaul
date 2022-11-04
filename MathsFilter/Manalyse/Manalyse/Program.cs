using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFilter;

namespace Manalyse
{
    internal class Program
    {
        static void Analyse(TransformMatrix transformMatrix)
        {
            TransformResults transformResults = new TransformResults(transformMatrix);
            System.Console.WriteLine($"Minimum {transformResults.Minumum}");
            System.Console.WriteLine($"     Q1 { transformResults.Q1}");
            System.Console.WriteLine($" Median { transformResults.Median}");
            System.Console.WriteLine($"     Q3 { transformResults.Q3}");
            System.Console.WriteLine($"Maximum {transformResults.Maximum}");
            System.Console.WriteLine($"   Mean {transformResults.Mean}");

            double Area = Convert.ToDouble(transformMatrix.Area);
            double negative = Convert.ToDouble(transformResults.Negative) * 100 / Area;
            double mono = Convert.ToDouble(transformResults.Mono) * 100 / Area;
            double duo = Convert.ToDouble(transformResults.Dou) * 100 / Area;
            double rgb = Convert.ToDouble(transformResults.RGB) * 100 / Area;
            double toohigh = Convert.ToDouble(transformResults.OutOfRange) * 100 / Area;

            System.Console.WriteLine();
            System.Console.WriteLine($"Negative {negative}");
            System.Console.WriteLine($"    Mono {mono}");
            System.Console.WriteLine($"    Duo  {duo}");
            System.Console.WriteLine($"    Rgb  {rgb}");
            System.Console.WriteLine($"Too High {toohigh}");
        }
        
        static void Main(string[] args)
        {
            string file = args[0];
            bool bcomplex = (args.Length > 1) && (args[1] == "-Complex" || args[1] == "-complex" || args[1] == "-co");
           if (bcomplex)
           {

           }
           else
            {
                TransformMatrix transformMatrix = new TransformMatrix();    
                transformMatrix.Load(file);

                Analyse(transformMatrix);

                TransformMatrix transformMatrix2 = transformMatrix * transformMatrix;

                Analyse(transformMatrix2);

              

                int p = 0;

            }
            
        
        }
    }
}
