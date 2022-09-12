﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoFilter
{
    public class ZMatrix
    {
        private ComplexNumber[,] _Pixels;
        private int _dimension;
        public double Scale { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }

     


        public ZMatrix(int dimension)
        {
            _dimension = dimension;
            int arraydim = 2 * dimension + 1;
            _Pixels = new ComplexNumber[arraydim, arraydim];
            for (int i = 0; i < arraydim; i++)
            {
                for (int j = 0; j < arraydim; j++)
                {
                    _Pixels[i, j] = new ComplexNumber();
                }
            }
        
        }

        public static ZMatrix operator +(ZMatrix t1, ZMatrix t2)
        {
            if (t1._dimension == t2._dimension)
            {
                ZMatrix tout = new ZMatrix(t1._dimension);
                for (int i = 0; i < tout.Dimension2; i++)
                {
                    for (int j = 0; j < tout.Dimension2; j++)
                    {
                        tout._Pixels[i, j] = t1._Pixels[i, j] + t2._Pixels[i, j];
                    }
                }
                return tout;
            }
            else
            {
                return null;
            }
        }

        public static ZMatrix operator -(ZMatrix t1, ZMatrix t2)
        {
            if (t1._dimension == t2._dimension)
            {
                ZMatrix tout = new ZMatrix(t1._dimension);
                for (int i = 0; i < tout.Dimension2; i++)
                {
                    for (int j = 0; j < tout.Dimension2; j++)
                    {
                        tout._Pixels[i, j] = t1._Pixels[i, j] - t2._Pixels[i, j];
                    }
                }
                return tout;
            }
            else
            {
                return null;
            }
        }

        public static ZMatrix operator *(ZMatrix t1, double d)
        {
            ZMatrix tout = new ZMatrix(t1._dimension);
            for (int i = 0; i < tout.Dimension2; i++)
            {
                for (int j = 0; j < tout.Dimension2; j++)
                {
                    tout._Pixels[i, j] = t1._Pixels[i, j] * d;
                }
            }
            return tout;
        }

        public static bool operator ==(ZMatrix t1, ZMatrix t2)
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

        public static bool operator !=(ZMatrix t1, ZMatrix t2)
        {
            return !(t1 == t2);
        }

        internal ComplexNumber[] Row(int iRow)
        {
            int Dim2 = Dimension2;
            ComplexNumber[] row = new ComplexNumber[Dim2];
            for (int i = 0; i < Dim2; i++)
            {
                row[i] = _Pixels[iRow, i];
            }
            return row;
        }

        internal ComplexNumber[] Column(int iCol)
        {
            int Dim2 = Dimension2;
            ComplexNumber[] Column = new ComplexNumber[Dim2];
            for (int i = 0; i < Dim2; i++)
            {
                Column[i] = _Pixels[i, iCol];
            }
            return Column;
        }

        public static ZMatrix operator *(ZMatrix t1, ZMatrix t2)
        {
            if (t1._dimension == t2._dimension)
            {
                Dictionary<int, ComplexNumber[]> ColList = new Dictionary<int, ComplexNumber[]>();
                ZMatrix tout = new ZMatrix(t1._dimension);
                for (int i = 0; i < tout.Dimension2; i++)
                {
                    ComplexNumber[] rowi = t1.Row(i);
                    for (int j = 0; j < tout.Dimension2; j++)
                    {
                        //row i of t1 * row j of t2
                        ComplexNumber[] colj;
                        if (ColList.ContainsKey(j))
                        {
                            colj = ColList[j];
                        }
                        else
                        {
                            colj = t2.Column(j);
                            ColList[j] = colj;
                        }

                        ComplexNumber f=new ComplexNumber();
                        for (int m = 0; m < tout.Dimension2; m++)
                        {
                            f += rowi[m] * colj[m];
                        }

                        tout._Pixels[i, j] = f;
                    }
                }
                return tout;
            }
            else
            {
                return null;
            }
        }

        public void Set(Func<ComplexNumber,ComplexNumber> f)
        {
            for (int i = -_dimension; i <= _dimension; i++)
            {
                for (int j = -_dimension; j <= _dimension; j += 1)
                {
                    ComplexNumber zin = new ComplexNumber((i+OffsetX)/Scale,(j+OffsetY)/Scale);
                    _Pixels[i + _dimension, _dimension - j] = f(zin);
                }
            }
        }

        TransformMatrix GetTransformMatrix(Func<ComplexNumber,double> t)
        {
            TransformMatrix tm = new TransformMatrix(Dimension);
            for (int i=0;i<Dimension2;i++)
            {
                for (int j=0;j<Dimension2;j++)
                {
                    double tval = t(_Pixels[i, j]);
                    tm[i, j] = tval;
                }
            }
            return tm;
        }

        public ComplexNumber this[int x, int y]
        {
            get
            {
                return _Pixels[x, y];
            }
        }

        int Dimension { get => _dimension; }
        int Dimension2 { get => _dimension * 2 + 1; }
        
    }
}
