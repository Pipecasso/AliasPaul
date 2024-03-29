﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.mariuszgromada.math.mxparser;


namespace GeoFilter
{

    public delegate double ProgressDelegate();

    public class TransformMatrix
    {
        private double[,] _Pixels;
        private int _dimension;
        private double _minumum;
        private double _maximum;
        private double _mid = Convert.ToDouble(Int32.MaxValue) - 256;
        private double _progress;

        private int _pulsegap;

        public TransformMatrix(int dimension, bool bIdendity = false)
        {
            _dimension = dimension;
            int arraydim = 2 * dimension + 1;
            _Pixels = new double[arraydim, arraydim];

            if (bIdendity)
            {
                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        _Pixels[i, j] = i == j ? 1 : 0;
                    }
                }

            }
        }


        public TransformMatrix(int dimension, double dval)
        {
            int arraydim = 2 * dimension + 1;
            double area1 = Convert.ToDouble(arraydim * arraydim) / 100;
            _pulsegap = Convert.ToInt32(Math.Floor(area1 + 0.5));
            _Pixels = new double[arraydim, arraydim];

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    _Pixels[i, j] = dval;
                }
            }

            _dimension = dimension;
            _minumum = dval;
            _maximum = dval;
        }

        public static TransformMatrix operator +(TransformMatrix t1, TransformMatrix t2)
        {
            if (t1._dimension == t2._dimension)
            {
                TransformMatrix tout = new TransformMatrix(t1._dimension);
                for (int i = 0; i < tout.Dimension2; i++)
                {
                    for (int j = 0; j < tout.Dimension2; j++)
                    {
                        tout._Pixels[i, j] = t1._Pixels[i, j] + t2._Pixels[i, j];
                    }
                }
                tout.SetMinMax();
                return tout;
            }
            else
            {
                return null;
            }
        }

        public static TransformMatrix operator -(TransformMatrix t1, TransformMatrix t2)
        {
            if (t1._dimension == t2._dimension)
            {
                TransformMatrix tout = new TransformMatrix(t1._dimension);
                for (int i = 0; i < tout.Dimension2; i++)
                {
                    for (int j = 0; j < tout.Dimension2; j++)
                    {
                        tout._Pixels[i, j] = t1._Pixels[i, j] - t2._Pixels[i, j];
                    }
                }
                tout.SetMinMax();
                return tout;
            }
            else
            {
                return null;
            }
        }

        public static TransformMatrix operator *(TransformMatrix t1, double d)
        {
            TransformMatrix tout = new TransformMatrix(t1._dimension);
            for (int i = 0; i < tout.Dimension2; i++)
            {
                for (int j = 0; j < tout.Dimension2; j++)
                {
                    tout._Pixels[i, j] = t1._Pixels[i, j] * d;
                }
            }
            return tout;
        }

        internal double[] Row(int iRow)
        {
            int Dim2 = Dimension2;
            double[] row = new double[Dim2];
            for (int i = 0; i < Dim2; i++)
            {
                row[i] = _Pixels[iRow, i];
            }
            return row;
        }

        internal double[] Column(int iCol)
        {
            int Dim2 = Dimension2;
            double[] Column = new double[Dim2];
            for (int i = 0; i < Dim2; i++)
            {
                Column[i] = _Pixels[i, iCol];
            }
            return Column;
        }
        #region staticsandoperators
        public static bool operator == (TransformMatrix t1, TransformMatrix t2)
        {
            if (t1.Dimension == t2.Dimension)
            {
                bool bRet = true;
                int i = 0;
                while (i < t1.Dimension && bRet)
                {
                    int j = 0;
                    while (j < t1.Dimension && bRet)
                    {
                        bRet = t1[i, j] == t2[i, j];
                        j++;
                    }
                    i++;
                }
                return bRet;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(TransformMatrix t1, TransformMatrix t2)
        {
            return !(t1 == t2);
        }


        public static TransformMatrix operator *(TransformMatrix t1, TransformMatrix t2)
        {
            if (t1._dimension == t2._dimension)
            {
                Dictionary<int, double[]> ColList = new Dictionary<int, double[]>();
                TransformMatrix tout = new TransformMatrix(t1._dimension);
                for (int i = 0; i < tout.Dimension2; i++)
                {
                    double[] rowi = t1.Row(i);
                    for (int j = 0; j < tout.Dimension2; j++)
                    {
                        //row i of t1 * row j of t2
                        double[] colj;
                        if (ColList.ContainsKey(j))
                        {
                            colj = ColList[j];
                        }
                        else
                        {
                            colj = t2.Column(j);
                            ColList[j] = colj;
                        }

                        double f = 0;
                        for (int m = 0;m< tout.Dimension2;m++)
                        {
                            f += rowi[m] * colj[m];
                        }

                        tout._Pixels[i, j] = f;
                    }
                }
                tout.SetMinMax();
                return tout;
            }
            else
            {
                return null;
            }
        }

        static public TransformMatrix Abs(TransformMatrix t)
        {
            
            TransformMatrix tout = new TransformMatrix(t.Dimension);
            int dim2 = t.Dimension2;
            for (int i=0;i<dim2;i++)
            {
                for (int j = 0; j < dim2; j++)
                {
                    tout._Pixels[i, j] = t[i, j] < 0 ? -1 * t[i, j] : t[i, j];
                }
            }
            tout.SetMinMax();
            return tout;

        }
        #endregion

        static public void ScaleToMatch(TransformMatrix t1, TransformMatrix t2)
        {
            if (t1.range > t2.range)
            {
                t1.TimesEquals(t2.range / t1.range);
            }
            else if (t1.range < t2.range)
            {
                t2.TimesEquals(t1.range / t2.range);
            }
            Debug.Assert(Math.Abs(t1.range - t2.range) < 1e-6);

        }

        public void TimesEquals (double d)
        {
            double dim2 = Dimension2;
            for (int i = 0; i < dim2; i++)
            {
                for (int j = 0; j < dim2; j++)
                {
                   
                    _Pixels[i, j] = _Pixels[i, j] * d;
                }
            }
            _minumum = _minumum * d;
            _maximum = _maximum * d;


        }

        public double Progress()
        {
            return _progress;
        }

        public void Set(Function f, bool bRadx = false, bool bRady = false)
        {
            double tiktok = 0;
            double total = Math.Pow(_dimension * 2 + 1, 2);
            _progress = 0;
            Pulse += new ProgressDelegate(Progress);
            _minumum = Int32.MaxValue;
            _maximum = Int32.MinValue;
            Argument x = new Argument("x", 0);
            Argument y = new Argument("y", 0);
            for (int i = -_dimension; i <= _dimension; i++)
            {
                x.setArgumentValue(bRadx ? i * Math.PI / 360 : i);
                for (int j = -_dimension; j <= _dimension; j++)
                {
                    y.setArgumentValue(bRady ? j * Math.PI / 360 : j);
                    double fval = f.calculate(x, y);

                    if (double.IsNaN(fval) || Math.Abs(fval) > _mid) fval = 0;
                    if (fval < _minumum)
                    {
                        _minumum = fval;
                    }

                    if (fval > _maximum)
                    {
                        _maximum = fval;
                    }

                    _Pixels[i + _dimension, _dimension - j] = fval;
                    tiktok++;
                    
                    if ( (tiktok % _pulsegap) == 0)
                    {
                        _progress = tiktok / total;
                        Pulse();
                    }
                    


                }
            }

            System.Diagnostics.Debug.Assert(_minumum <= _maximum);

        }

       
        

        public int Dimension
        {
            get
            {
                return _dimension;
            }
        }

        public int Dimension2
        {
            get
            {
                return _dimension * 2 + 1;
            }
        }

        public double this[int x, int y]
        {
            get
            {
                return _Pixels[x, y];
            }
        }

        public double CartesianGet(int i, int j)
        {
            return _Pixels[i + _dimension, _dimension - j];
        }

        public double minimum
        {
            get
            {
                return _minumum;
            }
        }

        public double maximum
        {
            get
            {
                return _maximum;
            }
        }

        public double range
        {
            get
            {
                return _maximum - _minumum;
            }
        }

        private void SetMinMax()
        {
            double dim2 = Dimension2;
             _maximum = double.MinValue;
             _minumum = double.MaxValue;
            for (int i = 0; i < dim2; i++)
            {
                for (int j = 0; j < dim2; j++)
                {
                    if (_Pixels[i, j] < _minumum)
                    {
                        _minumum = _Pixels[i, j];
                    }
                    if (_Pixels[i, j] > _maximum)
                    {
                        _maximum = _Pixels[i, j];
                    }
                }
            }

        }

        public double InRangeFactor(bool bRGB = false)
        {
            int tick = 0;
            int cross = 0;

            double dMax = bRGB ? Math.Pow(256, 3) - 1 : 255;
            double dMin = bRGB ? 0 : -255;
            int iDim = _dimension * 2 + 1;
            for (int i = 0; i < iDim; i++)
            {
                for (int j = 0; j < iDim; j++)
                {
                    double d = _Pixels[i, j];
                    if (d >= dMin && d <= dMax)
                    {
                        tick++;
                    }
                    else
                    {
                        cross++;
                    }

                }
            }
            Debug.Assert(tick + cross == iDim * iDim);
            return Convert.ToDouble(tick) / Convert.ToDouble(iDim * iDim);
        }

        public void FlatComposition(out double red, out double green, out double blue, out double outofrange)
        {
            int iDim = _dimension * 2 + 1;
            int ired = 0;
            int igreen = 0;
            int iblue = 0;
            int ioor = 0;
            double area = iDim * iDim;
            for (int i = 0; i < iDim; i++)
            {
                for (int j = 0; j < iDim; j++)
                {
                    double d = _Pixels[i, j];
                    int id = Convert.ToInt32(Math.Floor(d + 0.5));
                    if (id >= 0 && id < 256)
                    {
                        ired++;
                    }
                    else if (id >= 256 && id < 65536)
                    {
                        igreen++;
                    }
                    else if (id >= 65536 && id < 16777216)
                    {
                        iblue++;
                    }
                    else
                    {
                        ioor++;
                    }
                }
            }
            red = Convert.ToDouble(ired) / area;
            green = Convert.ToDouble(igreen) / area;
            blue = Convert.ToDouble(iblue) / area;
            outofrange = Convert.ToDouble(ioor) / area;
            Debug.Assert(Math.Abs(red + green + blue + outofrange - 1) < 1e-6);
        }

        public event ProgressDelegate Pulse;
    }

   
}
      
