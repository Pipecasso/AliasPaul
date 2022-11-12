using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFilter;
using System.Drawing;
using System.IO;
using Manalyse;

namespace MatrixPainter
{
    internal class Program
    {
        static PaintParameters CommandLine(string[] args)
        {
            PaintParameters pp = new PaintParameters();
            for (int i = 1; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-red": pp.RedMatrix = args[++i]; break;
                    case "-green": pp.GreenMatrix = args[++i]; break;
                    case "-blue": pp.BlueMatrix = args[++i]; break;
                    case "-all": pp.AllMatrix = args[++i]; break;
                    case "-stretch": pp.Stretch = true; break;
                    case "-stretch2": pp.Stretch = true; pp.StretchTop = System.Convert.ToInt32(args[++i]); break;
                }

            }
            return pp;
        }

        static void Stretch(TransformMatrix tm,int target_max)
        {     
            double mimimun = tm.minimum;
            double maximum = tm.maximum;
            double range = maximum - mimimun;
            Func<double, double> stretch_funk = (x) => { return x * target_max / range; };
            tm.ApplyFunction(stretch_funk);
          
        }

        static void Main(string[] args)
        {
            string picturepath = args[0];
            const int rgbmax = 256 * 256 * 256;

            PaintParameters pp = CommandLine(args);
            if (pp.All())
            {
                if (File.Exists(pp.AllMatrix))
                {
                    TransformMatrix tm = new TransformMatrix();
                    tm.Load(pp.AllMatrix);
                    BitmapBox box = new BitmapBox(Color.White,tm.Dimension2, tm.Dimension2);
                    if (pp.Stretch)
                    {
                        Stretch(tm, pp.StretchTop);
                    }
                    MatrixAnalysis manal = new MatrixAnalysis(tm);
                    Report report = new Report(manal);
                    report.Analyse();
                    box.ApplyMatrix(tm, tm.Dimension, tm.Dimension, rgbmax);
                    box.Save(picturepath);
                }
                else { Console.WriteLine($"{pp.AllMatrix} cannot be found."); }
            }
            else if (pp.RGB())
            {

            }


            /*  TransformMatrix tm = new TransformMatrix();
              tm.Load(matrixpath);
              BitmapBox bitmapBox = new BitmapBox(Color.White,tm.Dimension2,tm.Dimension2);
              bitmapBox.ApplyMatrix(tm, tm.Dimension, tm.Dimension);
              bitmapBox.Save(picturepath);*/
        }
    }
}
