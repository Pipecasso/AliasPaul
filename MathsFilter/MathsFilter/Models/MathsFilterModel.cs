using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.mariuszgromada.math.mxparser;
using System.Windows.Media.Imaging;
using GeoFilter;
using System.IO;

namespace MathsFilter.Models
{
    internal class MathsFilterModel
    {
        private Function _mainFunction;
        private Function _xFunc;
        private Function _yFunc;
        private BitmapImage _image;
        private int _xoffset;
        private int _yoffset;
        private int _scale;
    
        public int XOffset
        {
            get { return _xoffset; }
            set { _xoffset = value; }
        }

        public int YOffset
        {
            get { return _yoffset; }
            set { _yoffset = value; }
        }

        public int Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public void SetFunctionString(string funky)
        {
            string allfunc = $"f(x,y)={funky}";
            _mainFunction = new Function(allfunc);
        }

        public bool ValidFunction()
        {
            return _mainFunction != null && _mainFunction.checkSyntax();
        }

        public BitmapImage Image
        {
            get { return _image; }
        }
        public void Go()
        {

            int range = 700;
            TransformMatrix tm = new TransformMatrix(range, 0);
            tm.Set(_mainFunction);

            int size = range * 2 + 1;
            BitmapBox bitbox = new BitmapBox(System.Drawing.Color.Gray, size, size);
            BitmapBox.OutOfBounds oob = BitmapBox.OutOfBounds.Rollover;
            bitbox.ApplyMatrix(tm, size / 2, size / 2, BitmapBox.Colour.Red, oob);

            bitbox.Save("Hello.bmp");

            MemoryStream ms = new MemoryStream();
            bitbox.bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Position = 0;
            _image = new BitmapImage();
            _image.BeginInit();
            _image.StreamSource = ms;
            _image.EndInit();
        }
    }
}
