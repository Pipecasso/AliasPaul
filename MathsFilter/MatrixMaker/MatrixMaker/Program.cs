using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFilter;
using Manalyse;

namespace MatrixMaker
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

        static Parameters CommandLine(string[] args)
        {
           
            Parameters parameters = new Parameters();
            parameters.matrixpath = args[0];
            parameters.picturename = args[1];

            for (int i=2;i<args.Length;i++)
            {
                switch (args[i])
                {
                    case "-dim": parameters.Dimension =  Convert.ToInt32(args[i++]); break;
                    case "-scale": parameters.Scale = Convert.ToDouble(args[i++]); break;
                    case "-XO": parameters.XOffset = Convert.ToInt32(args[i++]); break;
                    case "-YO": parameters.YOffset = Convert.ToInt32(args[i++]); break;
                    case "-f": parameters.FunctionName = args[i++]; break;
                    case "-a": parameters.a = Convert.ToDouble(args[i++]);break;
                    case "-b": parameters.b = Convert.ToDouble(args[i++]); break;
                }

            }

            return parameters;
        }
        

        static void Main(string[] args)
        {
            TransformMatrix tm = new TransformMatrix();
            Functions funky = new Functions(); 
            Parameters parameters = CommandLine(args);
            Func<double, double, double, double, double> fu = funky.GetFunction(parameters.FunctionName);
            tm.Set(fu, parameters.FunctionName, parameters.XOffset, parameters.YOffset, parameters.Scale);
      
        }
    }
}
