using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFilter;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Drawing;
using System.IO;
namespace VectorBoy
{
    class Program
    {


        static Func<ColourVector, ColourVector, ColourVector> vectporproduct = (v1, v2) =>
        {
           
            ColourVector cvout = ColourVector.CrossProduct(v1, v2);
          //  ColourVector cvout2 = new ColourVector(cvout.Multiply(255));
            return cvout;
        };


        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");



            Func<DenseMatrix, DenseMatrix, ColourVector, ColourVector> fm = (m1, m2, v) => new ColourVector((m1.Multiply(m2)).Multiply(v));
            Func<DenseMatrix, DenseMatrix, DenseMatrix, ColourVector, ColourVector> fm2 = (m1, m2, m3, v) =>
            {
                DenseMatrix mtemp1 = (DenseMatrix)m1.Multiply(m2);
                DenseMatrix mtemp2 = (DenseMatrix)mtemp1.Multiply(m3);
                DenseVector dv = (DenseVector)mtemp2.Multiply(v);
                return new ColourVector(dv);
            };

            Func<double, double, double, double> PaulsRandomFunction = (r, g, b) =>
            {
                double dr = r * Math.PI / 180;
                double dg  =g * Math.PI / 180;
                double db = b * Math.PI / 180;

                return Math.Sin(r) + Math.Cos(g) + Math.Sin(b) * Math.Cos(b);
            };

            Func<ColourVector, ColourVector, bool> colourbatte1 = (cv1, cv2) =>
             {
                 double c1 = cv1.FuncMe(PaulsRandomFunction);
                 double c2 = cv2.FuncMe(PaulsRandomFunction);
                 return c1 > c2;

             };

           BitmapBox bb = new BitmapBox(args[0].ToString());
           VectorBox vb = bb.MakeVectorBox();

            Func<VectorBox, GeoPixel, GeoPixel, double, bool> Radius = (vbox, p, q, r) =>
            {
                if (vbox.InRange(q))
                {
                    ColourVector cvp = vbox[p];
                    ColourVector cvq = vbox[q];
                    DenseVector diff = cvp - cvq;
                    double mag = diff.L2Norm();
                    return mag < r;
                }
                else
                {
                    return false;
                }

            };


            int rad = Convert.ToInt32(args[1]);
            int ix = Convert.ToInt32(args[2]);
            int iy = Convert.ToInt32(args[3]);
            Console.WriteLine("Here we go!");
            
            DateTime d1 = DateTime.Now;
            
            GeoPixel origin = new GeoPixel(ix, iy);

            /* PixelBag pb = new PixelBag(origin,vb, rad, Radius,false);
             DateTime d2 = DateTime.Now;
             Console.WriteLine("Finished in time of {0} ", (d2 - d1).TotalSeconds);
             Console.WriteLine("Pixels {0}", pb.Pixels.Count);
             Console.WriteLine("Closed Borders {0}", pb.ClosedBorders.Count);
             Console.WriteLine("Neighbours {0}", pb.BagNeighbours.Count);*/
            /*BagOfBags bob = new BagOfBags(origin, vb,rad, Radius);
            Bitmap finalbp = bob.Process();
            finalbp.Save("WasitWorthit.png");*/


            VectorBox vbrandy = new VectorBox(1400, 1000);
            vbrandy.Randy();
            BitmapBox bbr = new BitmapBox(vbrandy);
            bbr.Save("randy.bmp");



            /*  BitmapBox bb1 = new BitmapBox(vbCopy,null,BitmapBox.OutOfBounds.Rollover);
              string filename = Path.GetFileNameWithoutExtension(args[0]);
              filename += "battle.jpg";
              string newname = Path.Combine(Path.GetDirectoryName(args[0]), filename);


              bb1.Save(newname);*/



        }
    }
}
