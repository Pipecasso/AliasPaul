﻿using System;
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

        public int FinalCol2(int val,OutOfBounds oob,int uppoerbound = 16777216)
        {
            int output = 0;
            if (val < 0 || val >= uppoerbound)
            {
                switch (oob)
                {
                    case OutOfBounds.Reject:
                        output = 0;
                        break;
                    case OutOfBounds.Rollover:
                        if (val >= uppoerbound)
                        { 
                            output = val % uppoerbound;
                        }
                        else 
                        {
                            int below = Math.Abs(val) % uppoerbound;
                            output = uppoerbound - below - 1;
                        }
                        break;
                    case OutOfBounds.Bounce:
                        if (val >= uppoerbound)
                        {
                            int above = val % uppoerbound;
                            output = uppoerbound - above;
                        }
                        else 
                        {
                            int below = Math.Abs(val) % uppoerbound;
                            output = below;
                        }
                        break;
                    case OutOfBounds.Stop:
                        if (val >= uppoerbound)
                        {
                            output = uppoerbound-1;
                        }
                        else 
                        {
                            output = 0;
                        }
                        break;
                    case OutOfBounds.Normalise:
                        output = val;
                        break;
                }
            }
            else
            {
                output = val;
            }
            return output;
        }

        public int FinalCold(double val,OutOfBounds oob,int upperbound = 16777216)
        {
            int ival = System.Convert.ToInt32(Math.Floor(val + 0.5));
            return FinalCol2(ival,oob,upperbound);
        }

        public void ApplyMatrix(TransformMatrix m, int x, int y,int upperbound = 16777216)
        {
            for (int i = -m.Dimension; i <= m.Dimension; i++)
            {
                for (int j = -m.Dimension; j <= m.Dimension; j++)
                {

                    double rgb = m[i + m.Dimension, m.Dimension - j];
                    int irgb = Convert.ToInt32(Math.Floor(rgb + 0.5));
                    if (irgb < -upperbound)
                    {
                        int absrgb = irgb * -1;
                        int temprgb = absrgb & upperbound;
                        irgb = upperbound + temprgb;
                    }
                    else if (irgb < 0)
                    {
                        irgb = upperbound + irgb;
                    }
                    else if (irgb >= upperbound)
                    {
                        irgb = irgb % upperbound;
                    }
                    
                    Color cnew = FromRGB(irgb);
                    _bitmap.SetPixel(x + i, y - j, cnew);
                }
            }
        }
     
        public void ApplyMatrix(TransformMatrix rm,TransformMatrix gm,TransformMatrix bm,OutOfBounds oob = OutOfBounds.Rollover)
        {
            int dimmax = new TransformMatrix[3] { rm, gm, bm }.Max(z => z.Dimension);
            for (int i=-dimmax;i<dimmax;i++)
            {
                int i2 = Math.Abs(i);
                for (int j = -dimmax; j < dimmax; j++)
                {
                    int r = 0;
                    int g = 0;
                    int b = 0;
                    int j2 = Math.Abs(j);

                    if (rm!=null && i2 <= rm.Dimension && j2 <= rm.Dimension)
                    {
                        r = FinalCold(rm[i, j], oob, 256);
                    }

                    if (gm != null && i2 <= gm.Dimension && j2 <= gm.Dimension)
                    {
                        g = FinalCold(gm[i, j], oob, 256);
                    }

                    if (bm != null && i2 <= bm.Dimension && j2 <= bm.Dimension)
                    {
                        b = FinalCold(bm[i, j], oob, 256);
                    }
                    Color cnew = Color.FromArgb(r, g, b);
                    _bitmap.SetPixel(i + dimmax, j + dimmax, cnew);
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
