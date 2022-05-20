using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace GeoFilter
{
    public class VectorBox
    {
        private ColourVector[,] _cv;
        private int _w;
        private int _h;
    
        public VectorBox(int x,int y)
        {
            _cv = new ColourVector[x, y];
            _w = x;
            _h = y;
        }

        public void Randy()
        {
            Random randy = new Random();
            for (int i = 0; i < _w; i++)
            {
                for (int j = 0; j < _h; j++)
                {
                    ColourVector cv = new ColourVector(randy.Next(0, 255), randy.Next(0, 255), randy.Next(0, 255));
                    _cv[i, j] = cv;
                }
            }
        }

        public VectorBox(Bitmap bmap)
        {
            _w = bmap.Width;
            _h = bmap.Height;
            _cv = new ColourVector[_w, _h];
            for (int i = 0; i < _w; i++)
            {
                for (int j = 0; j < _h; j++)
                {
                    Color c = bmap.GetPixel(i, j);
                    ColourVector cv = new ColourVector(c);
                    _cv[i, j] = cv;
                }
            }
        }



        public void DoStuff()
        {
          Func < ColourVector, ColourVector> normalise = v =>
          {
              Vector v2 = (Vector)v.Normalize(2);
              Vector v3 = (Vector)v2.Multiply(255);
              return new ColourVector(v3);

          };


            for (int i = 0; i < _w; i++)
            {
                for (int j = 0; j < _h; j++)
                {
                    ColourVector cv = _cv[i, j];
                    ColourVector v2 = normalise(cv);
                    _cv[i, j] = new ColourVector(v2);


                }
            }
        }

        public static DenseMatrix Red(double theta)
        {
            DenseMatrix rotate = new DenseMatrix(3, 3);
            rotate[0, 0] = 1;
            rotate[1, 1] = Math.Cos(theta);
            rotate[2, 2] = Math.Cos(theta);
            rotate[1, 2] = -Math.Sin(theta);
            rotate[2, 1] = Math.Sin(theta);
            rotate[0, 1] = 0;
            rotate[0, 2] = 0;
            rotate[1, 0] = 0;
            rotate[2, 0] = 0;

            return rotate;

        }

        public static DenseMatrix Green(double theta)
        {
            DenseMatrix rotate = new DenseMatrix(3, 3);
            rotate[0, 0] = Math.Cos(theta);
            rotate[1, 1] = 1;
            rotate[2, 2] = Math.Cos(theta);
            rotate[2, 0] = -Math.Sin(theta);
            rotate[0, 2] = Math.Sin(theta);
            rotate[1, 0] = 0;
            rotate[0, 1] = 0;
            rotate[1, 2] = 0;
            rotate[2, 1] = 0;
            return rotate;
        }

        public static DenseMatrix Blue(double theta)
        {
            DenseMatrix rotate = new DenseMatrix(3, 3);
            rotate[0, 0] = Math.Cos(theta);
            rotate[0, 1] = -Math.Sin(theta);
            rotate[0, 2] = 0;
            rotate[1, 0] = Math.Sin(theta);
            rotate[1, 1] = Math.Cos(theta);
            rotate[2, 2] = 1;
            rotate[2, 0] = 0;
            rotate[1, 2] = 0;
            rotate[2, 1] = 0;
            return rotate;

        }

        public void DoubleRotation(Func<DenseMatrix, DenseMatrix,ColourVector,ColourVector> DoubleR,DenseMatrix m1, DenseMatrix m2)
        {
            for (int i = 0; i < _w; i++)
            {
                for (int j = 0; j < _h; j++)
                {
                    ColourVector cvin = _cv[i, j];
                    ColourVector cvout = DoubleR(m1,m2, cvin);
                    _cv[i, j] = new ColourVector(cvout);
                }
            }
        }

        public void TripleRotation(Func<DenseMatrix, DenseMatrix, DenseMatrix, ColourVector, ColourVector> TripleR, DenseMatrix m1, DenseMatrix m2, DenseMatrix m3)
        {
            for (int i = 0; i < _w; i++)
            {
                for (int j = 0; j < _h; j++)
                {
                    ColourVector cvin = _cv[i, j];
                    ColourVector cvout = TripleR(m1, m2,m3, cvin);
                    _cv[i, j] = new ColourVector(cvout);
                }
            }


        }


        public void RotateRed(double dTheta)
        {
            DenseMatrix rotate = Red(dTheta);

            for (int i = 0; i < _w; i++)
            {
                for (int j = 0; j < _h; j++)
                {
                    ColourVector cvin = _cv[i, j];
                    ColourVector cvout = new ColourVector(rotate.Multiply(cvin));
                    _cv[i, j] = new ColourVector(cvout);
                }
            }
        }

        public void RotateGreen(double dTheta)
        {
            DenseMatrix rotate = Green(dTheta);
          
            for (int i = 0; i < _w; i++)
            {
                for (int j = 0; j < _h; j++)
                {
                    ColourVector cvin = _cv[i, j];
                    ColourVector cvout = new ColourVector(rotate.Multiply(cvin));
                    _cv[i, j] = new ColourVector(cvout);
                }
            }
        }

        public void RotateBlue(double dTheta)
        {

            DenseMatrix rotate = Blue(dTheta);



            for (int i = 0; i < _w; i++)
            {
                for (int j = 0; j < _h; j++)
                {
                    ColourVector cvin = _cv[i, j];
                    ColourVector cvout = new ColourVector(rotate.Multiply(cvin));
                    _cv[i, j] = new ColourVector(cvout);
                }
            }

        }
        public void DoSomethingWithaVector(ColourVector cv, Func<ColourVector, ColourVector, ColourVector> funky)
        {
   
            for (int i = 0; i < _w; i++)
            {
                for (int j = 0; j < _h; j++)
                {
                    ColourVector cvin = _cv[i, j];
                    ColourVector cvout = funky(cvin,cv);
                    _cv[i, j] = new ColourVector(cvout);
                }
            }
        }

        public void PixelBattle(GeoPixel left, GeoPixel right, Func<ColourVector, ColourVector, bool> funky)
        {
            ColourVector cvleft = this[left];
            ColourVector cvright = this[right];
            if (cvleft.ColourBattle(cvright,funky))
            {
                this[right] = cvleft;
            }
            else
            {
                this[left] = cvright;
            }
                 
        }

        public ColourVector this[GeoPixel gp]
        {
            get
            {
                return _cv[gp.x, gp.y];
            }
            set
            {
                _cv[gp.x, gp.y] = value;
            }
        }

        public ColourVector this[int x, int y]
        {
            get
            {
                return _cv[x, y];
            }
        }


        public int Height
        {
            get
            {
                return _h;
            }
        }

        public int Width
        {
            get
            {
                return _w;
            }
        }

        public VectorBox Clone()
        {
            VectorBox dolly = new VectorBox(_w, _h);
            for (int i=0;i<_w;i++)
            {
                for (int j=0;j<_h;j++)
                {
                    dolly._cv[i,j] = _cv[i, j];
                }
            }
            return dolly;
        }

        #region stretching
        public VectorBox DoubleClone()
        {
            VectorBox dolly = new VectorBox(_w*2, _h*2);
            for (int i = 0; i < _w; i++)
            {
                for (int j = 0; j < _h; j++)
                {
                    dolly._cv[i*2, j*2] = _cv[i, j];
                }
            }
            return dolly;

        }

        public void FillInGaps(Func< List<ColourVector>, ColourVector> funky)
        {
          
            for (int i =0; i < _w; i++)
            {
                int jStep;
                int jStart;
                bool bEvenCol;
        
                
                if (i%2 == 0)
                {
                    bEvenCol = true;
                    jStep = 2;
                    jStart = 1;
                }
                else
                {
                    bEvenCol = false;
                    jStep = 1;
                    jStart = 0;
                }
                
               
                for (int j = jStart; j < _h; j+=jStep)
                {
                    List<GeoPixel> ActiveNeighbours = new List<GeoPixel>();
                    List < ColourVector > ColourNeighbours = new List<ColourVector>();
                    if (bEvenCol)
                    {
                        GeoPixel up = new GeoPixel(i, j - 1);
                        GeoPixel down = new GeoPixel(i, j + 1);
                        if (j > 0) ActiveNeighbours.Add(up);
                        if (j<_h-1) ActiveNeighbours.Add(down);
                    }
                    else
                    {
                        if (j%2 ==0)
                        {
                            GeoPixel left = new GeoPixel(i - 1, j);
                            GeoPixel right = new GeoPixel(i + 1, j);
                            ActiveNeighbours.Add(left);
                            if (i < _w - 1) ActiveNeighbours.Add(right);
                        }
                        else
                        {
                            GeoPixel topleft = new GeoPixel(i - 1, j-1);
                            GeoPixel topright = new GeoPixel(i + 1, j-1);
                            GeoPixel bottomleft = new GeoPixel(i - 1, j+1);
                            GeoPixel bottomright = new GeoPixel(i + 1, j+1);
                            ActiveNeighbours.Add(topleft);
                           
                         
                            if (i < _w - 1)
                            {
                                if (j > 0)
                                {
                                    ActiveNeighbours.Add(topright);
                                }
                                if (j < _h - 1)
                                {
                                    ActiveNeighbours.Add(bottomright);
                                }
                            }

                            if (j>0)
                            {
                                ActiveNeighbours.Add(topleft);
                            }
                            if (j < _h-1)
                            {
                                ActiveNeighbours.Add(bottomleft);
                            }

                        }
                    }

                   
                    foreach (GeoPixel gp in ActiveNeighbours)
                    {
                        ColourVector cv = this[gp];
                        System.Diagnostics.Debug.Assert(cv != null);
                        ColourNeighbours.Add(cv);
                    }
                    System.Diagnostics.Debug.Assert(ColourNeighbours != null && ColourNeighbours.Count > 0);
                    _cv[i, j] = funky(ColourNeighbours);

                }
            }

        }

        #endregion

        public static bool operator == (VectorBox left, VectorBox right)
        {
            bool bOk = true;
            if (left.Width == right.Width && left.Height == right.Height)
            {
                int i = 0;
                int j = 0;
                while (bOk && i < left._w)
                {
                    while (bOk && j < left._h)
                    {
                        bOk = left[i, j] == right[i, j];
                        j++;
                    }
                    i++;
                }
                return bOk;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(VectorBox left, VectorBox right)
        {
            return !(left == right);

        }

        public bool InRange(GeoPixel p)
        {
            return p.x >= 0 && p.y >= 0 && p.x < _w && p.y < _h;
        }








    }
}
