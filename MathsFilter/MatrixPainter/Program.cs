using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFilter;
using System.Drawing;

namespace MatrixPainter
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            string matrixpath = args[0];
            string picturepath = args[1];
          
            TransformMatrix tm = new TransformMatrix();
            tm.Load(matrixpath);
            BitmapBox bitmapBox = new BitmapBox(Color.White,tm.Dimension2,tm.Dimension2);
            bitmapBox.ApplyMatrix(tm, tm.Dimension, tm.Dimension);
            bitmapBox.Save(picturepath);
            
        }
    }
}
