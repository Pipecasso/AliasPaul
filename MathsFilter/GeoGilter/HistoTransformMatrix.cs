using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GeoFilter
{
    public class HistoTransformMatrix : TransformMatrix
    {
        private int[] _red;
        private int[] _green;
        private int[] _blue;

        private int[] _red2;
        private int[] _green2;
        private int[] _blue2;

        HashSet<int> _colours;
        private int _toolow;
        private int _inrange;
        private int _toohigh;
    
        public HistoTransformMatrix(int dimension, double dval) : base(dimension, dval)
        {
            _red = new int[256];
            _green = new int[256];
            _blue = new int[256];
            _red2 = new int[256];
            _green2 = new int[256];
            _blue2 = new int[256];
            _colours = new HashSet<int>();
            _toolow = 0;
            _inrange = 0;
            _toohigh = 0;
        }

        protected override void Record(double val)
        {
            int rgb = System.Convert.ToInt32(Math.Floor(val + 0.5));
            int finalrgb;
          
          
            if ( rgb < 0 || rgb >= 16777216)
            {
                ColourVector cv = new ColourVector(rgb, OutOfBounds.Rollover);
                int r = cv.IRed;
                int g = cv.IGreen;
                int b = cv.IBlue;

                _red2[r]++;
                _green2[g]++;
                _blue2[b]++;

                if (val < 0)
                {
                    _toolow++;
                }
                else if (val >= 16777216)
                {
                    _toohigh++;
                }
                finalrgb = cv.SingleRgb;
            }
            else
            {
                _inrange++;
                ColourVector cv = new ColourVector(rgb, OutOfBounds.Stop);
                int r = cv.IRed;
                int g = cv.IGreen;
                int b = cv.IBlue;
                _red[r]++;
                _green[g]++;
                _blue[b]++;

                _red2[r]++;
                _green2[g]++;
                _blue2[b]++;
                finalrgb = rgb;
            }

            if (!_colours.Contains(finalrgb)) { _colours.Add(finalrgb); }
        }

        public int[] Red { get => _red; }
        public int[] Green { get => _green; }
        public int[] Blue { get => _blue; }

        public int ColourCount { get => _colours.Count; }

        public void RangeValues(out double toolow,out double toohigh,out double inrange)
        {
            double totalval = Dimension2 * Dimension2;
            toolow = _toolow * 100 / totalval;
            toohigh = _toohigh * 100 / totalval;
            inrange = _inrange * 100 / totalval;
        }
    }
}
