using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;


namespace GeoFilter
{
    
    public class BitmapBox
    {
        private Bitmap _bitmap;
        private const int _rgbmax =16777215;
        private string _bitpath;

        [Flags]
        public enum Colour
        {
            None = 0,
            Red = 1,
            Green = 2,
            Blue = 4
        }

        public enum OutOfBounds
        {
            Reject,
            Stop,
            Rollover,
            Bounce,
            Normalise
        }


        public BitmapBox(string path)
        {
            using (Stream bitstream = File.Open(path, System.IO.FileMode.Open))
            {
                Image img = Image.FromStream(bitstream);
                _bitmap = new Bitmap(img);
            }
            _bitpath = path;
        }

        public BitmapBox(Color c,int iWidth,int iHeight)
        {
            _bitmap = new Bitmap(iWidth, iHeight);
            for (int i=0;i<iWidth; i++)
            {
                for (int j=0;j<iHeight;j++)
                {
                    _bitmap.SetPixel(i, j, c);
                }
            }

            
        }

        public BitmapBox(VectorBox vb,Bitmap original = null,OutOfBounds oob = OutOfBounds.Reject)
        {
            int iTickTock = 0;
            _bitmap = new Bitmap(vb.Width, vb.Height);
            for (int i = 0; i < vb.Width; i++)
            {
                for (int j = 0; j < vb.Height; j++)
                {
                    ColourVector cv = vb[i, j];
                    Color c = Color.Black;
                    if (cv != null)
                    {
                        if (cv.ValidColour(ref c))
                        {
                            iTickTock++;
                        }
                        else
                        {
                            switch (oob)
                            {
                                case OutOfBounds.Reject:
                                    c = original.GetPixel(i, j);
                                    break;
                                case OutOfBounds.Stop:
                                case OutOfBounds.Rollover:
                                case OutOfBounds.Bounce:
                                    c = cv.RolloverColour();
                                    break;
                                case OutOfBounds.Normalise:
                                    c = cv.NormalisedColour();
                                    break;
                            }

                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.Assert(false);
                    }
                    _bitmap.SetPixel(i, j, c);
                }
            }
            double dp = Convert.ToDouble(iTickTock) * 100 / Convert.ToDouble(vb.Width * vb.Height);

        }

        public string Bitpath
        {
            get
            {
                return _bitpath;
            }
        }

        public Bitmap PeekMap(int x,int y, int d)
        {
            Bitmap bm = new Bitmap(d * 2 +1 , d * 2 + 1);
            for (int i = x-d; i<=x+d;i++)
            {
                for (int j = y - d; j <= y + d; j++)
                {
                    Color p = _bitmap.GetPixel(i, j);
                    bm.SetPixel(i - x + d, j - y + d, p);
                }

            }
            return bm;

        }

        private int FinalCol(int delta, int start, OutOfBounds oob,int upperbound)
        {
            int final = start + delta;
            int mod = upperbound + 1;
            if (final < 0 || final > upperbound)
            {
                switch (oob)
                {
                    case OutOfBounds.Reject:
                        final = start;
                        break;
                    case OutOfBounds.Rollover:
                        if (final > upperbound)
                        {
                            final = final % mod;
                        }
                        else
                        {
                            int below = Math.Abs(final) % mod;
                            final = mod - below-1;
                        }
                        break;
                    case OutOfBounds.Stop:
                        if (final > upperbound)
                        {
                            final = upperbound;
                        }
                        else
                        {
                            final = 0;
                        }
                        break;
                    case OutOfBounds.Bounce:
                        if (final > upperbound)
                        {
                            int above = final % mod;
                            final = upperbound - above;
                        }
                        else
                        {
                            int below = Math.Abs(final) % mod;
                            final = below;
                        }
                        break;

                }

            }
            System.Diagnostics.Debug.Assert(final>=0 && final <= upperbound);
            return final;

        }

        public void ApplyMatrixAndFlatten(TransformMatrix m, int x, int y, OutOfBounds oob = OutOfBounds.Reject)
        {
            for (int i = -m.Dimension; i <= m.Dimension; i++)
            {
                for (int j = -m.Dimension; j <= m.Dimension; j++)
                {
                    Color c = _bitmap.GetPixel(x + i, y - j);
                    int rgb = ToRGB(c);
                    double delta = m[i + m.Dimension, m.Dimension - j];
                    int idelta = 0;
                    idelta = Convert.ToInt32(Math.Floor(delta + 0.5));
                    int rbgfinal = FinalCol(idelta, rgb, oob, _rgbmax);
                    Color cnew = FromRGB(rbgfinal);
                    _bitmap.SetPixel(x + i, y - j, cnew);
                }
            }
        }
        public void ApplyMatrix(TransformMatrix m, int x, int y, Colour cols, OutOfBounds oob)
        {
            for (int i = -m.Dimension; i <= m.Dimension; i++)
            {
                for (int j = -m.Dimension; j <= m.Dimension; j++)
                {
                    Color c = _bitmap.GetPixel(x + i, y - j);
                    int r = c.R;
                    int g = c.G;
                    int b = c.B;
                    double delta = m[i + m.Dimension, m.Dimension - j];
                    int idelta = 0;
                  
                     idelta = Convert.ToInt32(Math.Floor(delta + 0.5));
               
                    if ((cols & Colour.Red) == Colour.Red)
                    {
                        r = FinalCol(idelta, c.R, oob, 255);
                    }

                    if ((cols & Colour.Green) == Colour.Green)
                    {
                        g = FinalCol(idelta, c.G, oob, 255);
                    }

                    if ((cols & Colour.Blue) == Colour.Blue)
                    {
                        b = FinalCol(idelta, c.B, oob, 255);
                    }

                    Color cnew = Color.FromArgb(r, g, b);
                    _bitmap.SetPixel(x + i, y - j, cnew);
                   
                }
            }

        }

        internal void MinMax(int dimension,int x,int y, Colour cols,ref int min,ref int max)
        {
            min = Int32.MaxValue;
            max = Int32.MinValue;
            double totalred = 0;
            double totalgreen = 0;
            double totalblue = 0;

            for (int i = -dimension; i <= dimension; i++)
            {
                for (int j = -dimension; j <= dimension; j++)
                {
                    Color c = _bitmap.GetPixel(x + i, y - j);
                    totalred += c.R;
                    totalgreen += c.G;
                    totalblue += c.B;

                    if ((cols & Colour.Red) == Colour.Red) MinCheck(ref min, c.R);
                    if ((cols & Colour.Green) == Colour.Green) MinCheck(ref min, c.G);
                    if ((cols & Colour.Blue) == Colour.Blue) MinCheck(ref min, c.B);

                    if ((cols & Colour.Red) == Colour.Red) MaxCheck(ref max, c.R);
                    if ((cols & Colour.Green) == Colour.Green) MaxCheck(ref max, c.G);
                    if ((cols & Colour.Blue) == Colour.Blue) MaxCheck(ref max, c.B);
                }
            }

            double edgelength = dimension * 2 + 1;
            double pixcount = edgelength * edgelength;
            double avgred = totalred / pixcount;
            double avggreen = totalgreen / pixcount;
            double blue = totalblue / pixcount;



        }

        private void MinCheck(ref int dCurrentMin,int Val)
        {
            if (Val < dCurrentMin) dCurrentMin = Val;
        }

        private void MaxCheck(ref int dCurrentMax,int Val)
        {
            if (Val > dCurrentMax) dCurrentMax = Val;
        }

        public void ApplyFlatStretchMatrix(TransformMatrix m, int x, int y, Colour cols)
        {
            int bitmin = Int32.MaxValue;
            int bitmax = Int32.MinValue;
            for (int i = -m.Dimension; i <= m.Dimension; i++)
            {
                for (int j = -m.Dimension; j <= m.Dimension; j++)
                {
                    Color c = _bitmap.GetPixel(x + i, y - j);
                    int RGB = c.ToArgb();
                    if (RGB < bitmin) bitmin = RGB;
                    if (RGB > bitmax) bitmax = RGB;
                }
            }

            int mmax = Convert.ToInt32(Math.Floor(m.maximum + 0.5));
            int mmin = Convert.ToInt32(Math.Floor(m.minimum + 0.5));

            int itotalmax = mmax + bitmax;
            int itotalmin = mmin + bitmin;
            double dmult = Math.Pow(255,3) / (itotalmax - itotalmin);

            for (int i = -m.Dimension; i <= m.Dimension; i++)
            {
                for (int j = -m.Dimension; j <= m.Dimension; j++)
                {
                    Color c = _bitmap.GetPixel(x + i, y - j);
                    double val = (c.ToArgb() + m[i, j] - itotalmin)*dmult;
                    Color newc = Color.FromArgb(Convert.ToInt32(val + Math.Floor(val)));
                    _bitmap.SetPixel(x + i, y - j, newc);
                }
            }
        }

        

        public void ApplyStretchMatrix(TransformMatrix m, int x, int y, Colour cols)
        {
            int bitmin = Int32.MaxValue;
            int bitmax = Int32.MinValue;


            MinMax(m.Dimension, x, y, cols, ref bitmin, ref bitmax);

            int mmax = Convert.ToInt32(Math.Floor(m.maximum + 0.5));
            int mmin = Convert.ToInt32(Math.Floor(m.minimum + 0.5));

            int itotalmax = mmax + bitmax;
            int itotalmin = mmin + bitmin;
            double drange = Convert.ToDouble(itotalmax - itotalmin);
            double dmult = 255 / drange;

            for (int i = -m.Dimension; i <= m.Dimension; i++)
            {
                for (int j = -m.Dimension; j <= m.Dimension; j++)
                {
                    Color c = _bitmap.GetPixel(x + i, y - j);
                    int r = c.R;
                    int g = c.G;
                    int b = c.B;

           
                    double v = m[i + m.Dimension, m.Dimension - j];
                 
                    if ((cols & Colour.Red) == Colour.Red)
                    {
                        double val = (r + v - itotalmin) * dmult;
                        r = Convert.ToInt32(Math.Floor(val + 0.5));
                    }
                    if ((cols & Colour.Green) == Colour.Green)
                    {
                        double val = (g + v - itotalmin) * dmult;
                        g = Convert.ToInt32(Math.Floor(val + 0.5));
                    }
                    if ((cols & Colour.Blue) == Colour.Blue)
                    {
                        double val = (b + v - itotalmin) * dmult;
                        b = Convert.ToInt32(Math.Floor(val + 0.5));
                    }
                    _bitmap.SetPixel(x + i, y - j, Color.FromArgb(r, g, b));

                }
            }


        }

        private int ToRGB(Color c)
        {
            return ToRGB(c.R, c.G, c.B);
        }

        private int ToRGB(int r,int g,int b)
        {
            return r + g * 256 + b * 65536;
        }

        private void FromRGB(int rgb,out int r,out int g, out int b)
        {
            b = rgb / 65536;
            int rg = rgb % 65536;
            g = rg / 256;
            r = rg % 256;
        }

        private Color FromRGB(int rgb)
        {
            int r;
            int g;
            int b;
            FromRGB(rgb, out r, out g, out b);
            return Color.FromArgb(r, g, b);

        }

        public void Save(string spath)
        {
            _bitmap.Save(spath);
        }

        public Bitmap bitmap
        {
            get
            {
                return _bitmap;
            }
        }

        public VectorBox MakeVectorBox()
        {
            VectorBox vb1 = new VectorBox(_bitmap);
            return vb1;
        }
    }
}
