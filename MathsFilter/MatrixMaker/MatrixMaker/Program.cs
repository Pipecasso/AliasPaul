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


        static Parameters CommandLine(string[] args)
        {
           
            Parameters parameters = new Parameters();

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-dim": parameters.Dimension =  Convert.ToInt32(args[++i]); break;
                    case "-scale": parameters.Scale = Convert.ToDouble(args[++i]); break;
                    case "-XO": parameters.XOffset = Convert.ToInt32(args[++i]); break;
                    case "-YO": parameters.YOffset = Convert.ToInt32(args[++i]); break;
                    case "-f": parameters.FunctionName = args[++i]; break;
                    case "-a": parameters.a = Convert.ToDouble(args[++i]);break;
                    case "-b": parameters.b = Convert.ToDouble(args[++i]); break;
                    case "-save": parameters.picturename = args[++i]; break;
                    case "-pow": parameters.power = Convert.ToUInt32(args[++i]);break;
                    default:parameters.AddMessage($"Unknwon Parameter {args[i]}");break;
                }

            }

            return parameters;
        }
        

        static void Main(string[] args)
        {

            Functions funky = new Functions(); 
            Parameters parameters = CommandLine(args);
            TransformMatrix tm = new TransformMatrix(parameters.Dimension);
            Func<double, double, double, double, double> fu = funky.GetFunction(parameters.FunctionName);
            tm.Set(fu, parameters.FunctionName, parameters.XOffset, parameters.YOffset, parameters.Scale,parameters.a,parameters.b);
            if (parameters.power > 1)
            {
                for (int i=1 ; i<parameters.power; i++)
                {
                    tm = tm * tm;
                }
                tm.StringRepFunction = $"{parameters.FunctionName}pow{parameters.power}";
            }

            if (!funky.HasFunction(parameters.FunctionName))
            {
                System.Console.WriteLine($"Function {parameters.FunctionName} not found - default to circle");
            }

            MatrixAnalysis ma = new MatrixAnalysis(tm);
            Report rp = new Report(ma);
            rp.Analyse();
            if (parameters.picturename != string.Empty)
            {
                tm.Save(parameters.picturename);
            }
             
        }
    }
}
