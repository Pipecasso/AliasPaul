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

        static void Stretch(TransformMatrix tm,int target_min,int target_max)
        {     
            double mimimun = tm.minimum;
            double maximum = tm.maximum;
            double range = maximum - mimimun;
            double range2 = target_max - target_min;
            Func<double, double> stretch_funk = (x) =>
            {
                return target_min + ((x - mimimun)*range2 / range);
            };
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
                        Stretch(tm, 0,pp.StretchTop);
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
                int dim2 = 0;
                TransformMatrix red_matrix = null;
                TransformMatrix green_matrix = null;
                TransformMatrix blue_matrix = null;

                MatrixAnalysis  redAnal,greenAnal,blueAnal;
                if (pp.RedMatrix != null)
                {
                    red_matrix = new TransformMatrix();
                    red_matrix.Load(pp.RedMatrix);
                    redAnal = new MatrixAnalysis(red_matrix);
                    Report report = new Report(redAnal);
                    report.Analyse("Red");
                    if (red_matrix.Dimension2 >  dim2) dim2 = red_matrix.Dimension2;
                    if (pp.Stretch)
                    {
                        Stretch(red_matrix, 0, 255);
                    }    
                }
                if (pp.GreenMatrix != null)
                {
                    green_matrix = new TransformMatrix();
                    green_matrix.Load(pp.GreenMatrix);
                    greenAnal = new MatrixAnalysis(green_matrix);
                    Report report = new Report(greenAnal);
                    report.Analyse("Green");
                    if (green_matrix.Dimension2 > dim2) dim2 = green_matrix.Dimension2;
                    if (pp.Stretch)
                    {
                        Stretch(green_matrix, 0, 255);
                    }
                }
                if (pp.BlueMatrix != null)
                {
                    blue_matrix = new TransformMatrix();
                    blue_matrix.Load(pp.BlueMatrix);    
                    blueAnal = new MatrixAnalysis(blue_matrix);
                    Report report = new Report(blueAnal);
                    report.Analyse("Blue"); 
                    if (blue_matrix.Dimension2 > dim2) dim2 = blue_matrix.Dimension2;
                    if (pp.Stretch)
                    {
                        Stretch(blue_matrix, 0, 255);
                    }
                }
                BitmapBox bitmapBox = new BitmapBox(Color.White, dim2 , dim2);
                bitmapBox.ApplyMatrix(red_matrix, green_matrix, blue_matrix);
                bitmapBox.Save(picturepath);
                
            }


            /*  TransformMatrix tm = new TransformMatrix();
              tm.Load(matrixpath);
              BitmapBox bitmapBox = new BitmapBox(Color.White,tm.Dimension2,tm.Dimension2);
              bitmapBox.ApplyMatrix(tm, tm.Dimension, tm.Dimension);
              bitmapBox.Save(picturepath);*/
        }
    }
}
