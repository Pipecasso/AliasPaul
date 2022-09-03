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
        private double _scale;

        private HistoTransformMatrix _transformMatrix;
        private BitmapBox _box;

        public MathsFilterModel()
        {
            _xoffset = 0;
            _yoffset = 0;
            _scale = 1;
            SetOffSetScaleFunctions();
        }
    
        public int XOffset
        {
            get { return _xoffset; }
            set { _xoffset = value; SetOffSetScaleFunctions(); }
        }

        public int YOffset
        {
            get { return _yoffset; }
            set { _yoffset = value; SetOffSetScaleFunctions(); }
        }

        public double Scale
        {
            get { return _scale; }
            set { _scale = value; SetOffSetScaleFunctions(); }
        }

      

        public Function MainFunction { get { return _mainFunction; } }

        private void SetOffSetScaleFunctions()
        {
            string xFunc = $"f(x)=(x+{_xoffset})/{_scale}";
            _xFunc = new Function(xFunc);
            string yFunc = $"g(y)=(y+{_xoffset})/{_scale}";
            _yFunc = new Function(yFunc);
        }

        public void SetFunctionString(string funky)
        { 
            string funkr = funky.Replace("x", "f(x)");
            funkr = funkr.Replace("y", "g(y)");
            string allfunc = $"h(x,y) = {funkr}";
            _mainFunction = new Function(allfunc,_xFunc,_yFunc);
        }

        public bool ValidFunction()
        {
            return _mainFunction != null && _mainFunction.checkSyntax();
        }

        public BitmapImage Image
        {
            get { return _image; }
        }

        public HistoTransformMatrix TransformMatrix { get { return _transformMatrix; } }
        public void InitialiseTransformMatrix(int range,uint gap)
        {
            _transformMatrix= new HistoTransformMatrix(range, 0);
            _transformMatrix.Gap = gap;
        }

        public void PaintImage()
        {
            int size = _transformMatrix.Dimension2;
            _box = new BitmapBox(System.Drawing.Color.Gray, size, size);
            OutOfBounds oob = OutOfBounds.Rollover;
            // bitbox.ApplyMatrix(_transformMatrix, size / 2, size / 2, BitmapBox.Colour.Red, oob);
            _box.ApplyMatrixAndFlatten(_transformMatrix, size / 2, size / 2, oob);
            //bitbox.Save("Hello.bmp");

            MemoryStream ms = new MemoryStream();
            _box.bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Position = 0;
            _image = new BitmapImage();
            _image.BeginInit();
            _image.StreamSource = ms;
            _image.EndInit(); 
        }

        public void SaveBox(string path)
        {
            _box.bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        

     
    }
}
