using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.mariuszgromada.math.mxparser;
using GeoFilter;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace GeoFilterClient
{



    class Program
    {

        internal abstract  class Transform
        {
            protected BitmapBox _bb;
            protected int _dimension;

            internal Transform(int dimension)
            {
                _dimension = dimension;
            }

            internal void ApplyMatrix(TransformMatrix t,BitmapBox.Colour c,BitmapBox.OutOfBounds oob)
            {
                int Dim2 = _dimension * 2 + 1;
                _bb = new BitmapBox(Color.Gray, Dim2, Dim2);
                _bb.ApplyMatrix(t, _dimension, _dimension, c, oob);
                _bb.Save("Hello.bmp");

            }

            internal void ApplyFlatMatrix(TransformMatrix t)
            {
                int Dim2 = _dimension * 2 + 1;
                _bb = new BitmapBox(Color.Gray, Dim2, Dim2);
                _bb.ApplyMatrixAndFlatten(t, _dimension, _dimension, BitmapBox.OutOfBounds.Rollover);
                _bb.Save("Hello.bmp");

            }

            public abstract void ApplyMatrix(BitmapBox.Colour c, BitmapBox.OutOfBounds oob);
            public abstract void ApplyFlatMatrix();

        }


        class SingleTransform : Transform
        {
          
            protected TransformMatrix _t;

            public SingleTransform(Function f,int Dimension, bool bRadians = false) : base(Dimension)
            {
   

                Console.WriteLine("First One");
                _t = new TransformMatrix(Dimension, 0);
                _t.Pulse += GetProgress;
                _t.Set(f, bRadians);
               
            }

            public double GetProgress()
            {
                double yah =  _t.Progress();
                Console.WriteLine("{0}%", yah * 100);
                return yah;
            }

            public override void ApplyMatrix(BitmapBox.Colour c, BitmapBox.OutOfBounds oob)
            {
                base.ApplyMatrix(_t, c, oob);

            }

            public override void ApplyFlatMatrix()
            {
                base.ApplyFlatMatrix(_t);
            }
        }

        class DoubleTransform: SingleTransform
        {
            private TransformMatrix _t2;
            private TransformMatrix _t3;

            public DoubleTransform(Function f, Function g, int Dim, bool bAdd) : base(f,Dim,false)
            {
                
                Console.WriteLine("Second One");

                _t2 = new TransformMatrix(Dim, 0);
                _t2.Pulse += GetProgress2;
                _t2.Set(g);

                Console.WriteLine("Transform");

                TransformMatrix.ScaleToMatch(_t, _t2);
                if (bAdd)
                {
                    _t3 = _t + _t2;
                }
                else
                {
                    _t3 = _t2 * _t;
                }
            }

            public double GetProgress2()
            {
                double yah = _t2.Progress();
                Console.WriteLine("{0}%", yah * 100);
                return yah;
            }

            public override void ApplyMatrix(BitmapBox.Colour c, BitmapBox.OutOfBounds oob)
            {
                Console.WriteLine("In range{0}", _t3.InRangeFactor() * 100);
                base.ApplyMatrix(_t3, c, oob);

            }

            public override void ApplyFlatMatrix()
            {
                double red;
                double green;
                double blue;
                double oob;

                _t3.FlatComposition(out red,out green, out blue, out oob);
                Console.WriteLine("red {0} green {1} blue{2} oob{3}", red * 100, green * 100, blue * 100, oob * 100);
                _t3 = TransformMatrix.Abs(_t3);
                double dRangeMult = _t3.maximum / (Math.Pow(256, 3) - 1);

                if (dRangeMult > 1) _t3.TimesEquals(1 / dRangeMult);
                _t3.FlatComposition(out red, out green, out blue, out oob);
                Console.WriteLine("red {0} green {1} blue{2} oob{3}", red * 100, green * 100, blue * 100, oob * 100);
                base.ApplyFlatMatrix(_t3);
              
            }

            public TransformMatrix T3
            {
                get
                {
                    return _t3;
                }
            }


        }

     
        static void Main(string[] args)
        {
            int iDim = 0;
            if (args.Length > 0)
            {
                iDim = Convert.ToInt32(args[0]);
            }
            else
            {
                iDim = 150;
            }

            
           

       
             //DoubleTransform dt = new DoubleTransform(Onion, YinYang, iDim, false);


            //dt.ApplyFlatMatrix();
            //dt.ApplyMatrix(BitmapBox.Colour.Blue | BitmapBox.Colour.Green| BitmapBox.Colour.Red, BitmapBox.OutOfBounds.Rollover);

          //  SingleTransform st = new SingleTransform(YinYang, iDim);
          //  st.ApplyMatrix(BitmapBox.Colour.Red | BitmapBox.Colour.Blue , BitmapBox.OutOfBounds.Rollover);


            //bb.Save("hello.bmp");
        }
    }
}
