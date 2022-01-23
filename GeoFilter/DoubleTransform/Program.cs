using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFilter;
using org.mariuszgromada.math.mxparser;
using TransformParameters;
using System.Drawing;


namespace DoubleTransform
{
    class  Program
    {

        private TransformMatrix _t1;
        private TransformMatrix _t2;
        private TransformMatrix _t3;
        
        public Program(Parameters p)
        {
            if (!p.Valid)
            {
                foreach (string s in p._errorlist)
                {
                    Console.WriteLine(s);
                }
                return;
            }
            
            
            _t1 = new TransformMatrix(p.dimension,0);
            _t1.Pulse += Pulse1;
            _t1.Set(p.f);

            if (p.IsDoubleTransform())
            {
                _t2 = new TransformMatrix(p.dimension, 0);
                _t2.Pulse += Pulse2;
                _t2.Set(p.g);
                Console.WriteLine("t1 Minimum {0} - Maximum {1} ", _t1.minimum, _t1.maximum);
                Console.WriteLine("t2 Minimum {0} - Maximum {1} ", _t2.minimum, _t2.maximum);
                if (p.ScaleToMatch)
                {
                    TransformMatrix.ScaleToMatch(_t1, _t2);
                    Console.WriteLine("Scale");
                    Console.WriteLine("t1 Minimum {0} - Maximum {1} ", _t1.minimum, _t1.maximum);
                    Console.WriteLine("t2 Minimum {0} - Maximum {1} ", _t2.minimum, _t2.maximum);
                }

                switch (p.moperation)
                {
                    case Parameters.MatrixOperation.add:
                        _t3 = _t1 + _t2;
                        break;
                    case Parameters.MatrixOperation.minus:
                        _t3 = _t1 - _t2;
                        break;
                    case Parameters.MatrixOperation.multiply:
                        _t3 = _t1 * _t2;
                        break;
                }


                if (p.abs) _t3 = TransformMatrix.Abs(_t3);
                if (p.Scale) _t3.TimesEquals(p.ScaleFactor);

                Console.WriteLine("Minimum {0} - Maximum {1} ", _t3.minimum, _t3.maximum);

                double dRangePerenct = _t3.InRangeFactor();
                Console.WriteLine("In range % {0:0.00} ", dRangePerenct * 100);

                double red, green, blue, oob;
                _t3.FlatComposition(out red, out green, out blue, out oob);
                Console.WriteLine("Flat Composition Red {0:0.00}% Green {1:0.00}% Blue {2:0.00}% OOB {3:0.00}% ", red * 100, green * 100, blue * 100, oob * 100);

                if (p.Standard)
                {
                    BitmapBox bmb = new BitmapBox(Color.Gray, _t1.Dimension2, _t1.Dimension2);
                    bmb.ApplyMatrix(_t3, _t1.Dimension, _t1.Dimension, p.Colours, p.oob);
                    bmb.Save("Standard.bmp");
                }

                if (p.Flat)
                {
                    BitmapBox bmb = new BitmapBox(Color.Gray, _t1.Dimension2, _t1.Dimension2);
                    bmb.ApplyMatrixAndFlatten(_t3, _t1.Dimension, _t1.Dimension, p.oob);
                    bmb.Save("Flat.bmp");

                }
            }
            else
            {
                Console.WriteLine("t1 Minimum {0} - Maximum {1} ", _t1.minimum, _t1.maximum);

                double dRangePerenct = _t1.InRangeFactor();
                Console.WriteLine("In range % {0:0.00} ", dRangePerenct * 100);

                double red, green, blue, oob;
                _t1.FlatComposition(out red, out green, out blue, out oob);
                Console.WriteLine("Flat Composition Red {0:0.00}% Green {1:0.00}% Blue {2:0.00}% OOB {3:0.00}% ", red * 100, green * 100, blue * 100, oob * 100);

                if (p.Standard)
                {
                    BitmapBox bmb = new BitmapBox(Color.Gray, _t1.Dimension2, _t1.Dimension2);
                    bmb.ApplyMatrix(_t1, _t1.Dimension, _t1.Dimension, p.Colours, p.oob);
                    bmb.Save("Standard.bmp");
                }

                if (p.Flat)
                {
                    BitmapBox bmb = new BitmapBox(Color.Gray, _t1.Dimension2, _t1.Dimension2);
                    bmb.ApplyMatrixAndFlatten(_t1, _t1.Dimension, _t1.Dimension, p.oob);
                    bmb.Save("Flat.bmp");

                }
            }
    


        }

        public double Pulse1()
        {
            double dp = _t1.Progress();
            Console.WriteLine("M1 {0:0.00}%", Math.Floor(dp * 100 + 0.5));
            return dp;
        }

        public double Pulse2()
        {
            double dp = _t2.Progress();
            Console.WriteLine("M2 {0:0.00}%", Math.Floor(dp * 100 + 0.5));
            return dp;
        }


        static void Main(string[] args)
        {
             TransformFunctions t1 = new TransformFunctions();
             Parameters param = new Parameters(args);
            Program p = new Program(param);

        
        }
    }
}
