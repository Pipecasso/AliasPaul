using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    public class Matrix22
    {
        public Matrix22(double a, double b, double c, double d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public Matrix22(double[] abcd)
        {
            this.a = abcd[0];
            this.b = abcd[1];
            this.c = abcd[2];
            this.d = abcd[3];
        }

        //[ a  b ]
        //[ c  d ]
        public double a { get; private set; }
        public double b { get; private set; }
        public double c { get; private set; }
        public double d { get; private set; }

        public double Determinant
        { get => a * d - b * c; }

        public Matrix22 Inverse()
        {
            Matrix22 togo = null;
            double determinant = Determinant;
            if (determinant != 0)
            {
                togo = new Matrix22(d / determinant, -b / determinant, -c / determinant, a / determinant);
            }
            return togo;
        }

        static public Vector2d operator *(Matrix22 m,Vector2d v)
        {
            return new Vector2d(m.a * v.Item1 + m.b * v.Item2, m.c * v.Item1 + m.d * v.Item2);

        }
    }

    public class Matrix33
    {
        private double[,] _Values;

        public Matrix33()
        {
            _Values = new double[3, 3];
            for (int i =0; i<3;i++)
            {
                for (int j=0;j<3;j++)
                {
                    _Values[i, j] = 0;
                }
            }
        }
            
        public Matrix33(Vector3d v1,Vector3d v2,Vector3d v3,bool bRow)
        {
            _Values = new double[3, 3];
            Vector3d[] vectors = new Vector3d[] { v1, v2, v3 };

            if (bRow)
            {
                for (int i = 0; i < 3; i++)
                {
                    _Values[0, i] = vectors[i].X;
                    _Values[1, i] = vectors[i].Y;
                    _Values[2, i] = vectors[i].Z;
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    _Values[i, 0] = vectors[i].X;
                    _Values[i, 1] = vectors[i].Y;
                    _Values[i, 2] = vectors[i].Z;
                }
            }
        }
      
        public double this[int x, int y]
        {
            get
            {
                return _Values[x, y];
            }
            set
            {
                _Values[x, y] = value;
            }
        }

        public Vector3d row(int y)
        {
            Vector3d vout = new Vector3d(this[0, y], this[1, y], this[2, y]);
            return vout;
        }

        public Vector3d column(int x)
        {
            return new Vector3d(this[x, 0], this[x, 1], this[x, 2]);
        }
        static public Vector3d operator * (Matrix33 m,Vector3d v)
        {
            double x = m[0, 0] * v.X + m[1, 0] * v.Y + m[2, 0] * v.Z;
            double y = m[0, 1] * v.X + m[1, 1] * v.Y + m[2, 1] * v.Z;
            double z = m[0, 2] * v.X + m[1, 2] * v.Y + m[2, 2] * v.Z;
            return new Vector3d(x, y, z);
        }

        static public Matrix33 operator * (Matrix33 m, double d)
        {
            Matrix33 mout = new Matrix33();
            for (int i=0;i<3;i++)
            {
                for (int j=0;j<3;j++)
                {
                    mout[i, j] = m[i, j] * d;
                }
            }
            return mout;
        }

        static public Matrix33 operator / (Matrix33 m,double d)
        {
            return m * (1 / d);
        }

        static public bool operator == (Matrix33 l,Matrix33 r)
        {
            bool yestheyare = true;
            int i = 0;
            int j = 0;
            while (yestheyare && i < 3)
            {
                j = 0;
                while (yestheyare && j < 3)
                {
                    if (l[i, j] == r[i, j])
                    {
                        j++;
                    }
                    else
                    {
                        yestheyare = false;
                    }
                }
                i++;
            }
            return yestheyare;
        }

        static public bool operator != (Matrix33 l,Matrix33 r)
        {
            return !(l == r);
        }

        public bool IsIdentity(double tolerance = 0)
        {
            Func<int, int, double, bool> IdentityTest = (x, y, target) => { return (Math.Abs(this[x, y] - target) <= tolerance); };


            bool yesitis = true;
            int i = 0;
            int j = 0;
            while (yesitis && i < 3)
            {
                j = 0;
                while (yesitis && j < 3)
                {
                    if  (  (i==j && IdentityTest(i,j,1)) || (i!=j && IdentityTest(i,j,0)))
                    {
                        j++;
                    }
                    else
                    {
                        yesitis = false;
                    }
                }
                i++;
            }
            return yesitis;
        }

        private Matrix22 SubMatrix(int x,int y)
        {
            int stage = 0;
            double[] abcd = { 0, 0, 0, 0 };
            for (int j = 0; j < 3; j++)
            {
                if (j == y) continue;
                for (int i = 0; i < 3; i++)
                {
                    if (i == x) continue;
                    abcd[stage] = this[i, j];
                    stage++;
                }
            }
            return new Matrix22(abcd);
        }

        public double Determinant
        {
            get => this[0, 0] * SubMatrix(0, 0).Determinant - this[1, 0] * SubMatrix(1, 0).Determinant + this[2, 0] * SubMatrix(2, 0).Determinant;         
        }

        static public Matrix33 Inverse(Matrix33 m)
        {
            Func<int, int, double> Conjugate = (i, j) => { return Math.Pow(-1, i + j) * m.SubMatrix(i, j).Determinant; };
            Matrix33 conj = new Matrix33();
            for (int j = 0; j < 3; j++)
            {
                for (int i=0;i<3;i++)
                {
                    conj[i, j] = Conjugate(i, j);
                }
            }

            Matrix33 conjt = Transpose(conj); // for understanding purposes this could be optimised out 
          
            Matrix33 inverse = conjt / m.Determinant;
            return inverse;
        }

        static public Matrix33 Transpose(Matrix33 m)
        {
            Matrix33 mout = new Matrix33();
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    mout[j, i] = m[i, j];
                }
            }
            return mout;

        }

        static public Matrix33 operator * (Matrix33 l, Matrix33 r)
        {
            Matrix33 mout = new Matrix33();
            for (int j = 0; j < 3; j++)
            {
                Vector3d vrow = l.row(j);
                for (int i = 0; i < 3; i++)
                {
                    Vector3d vcol = r.column(i);
                    double val = Vector3d.Dot(vrow, vcol);
                    mout[i, j] = val;
                
                }
            }
            return mout;
        }

        //Use Equation3 instead, it uses Cramer's rule which is faster
        static public bool SolveSystemOfEquations(Matrix33 m,Vector3d v,out double x,out double y,out double z)
        { 
            
            if (m.Determinant == 0)
            {
                x = 0;
                y = 0;
                z = 0;
                return false;
            }
            else
            {
                Matrix33 minv = Matrix33.Inverse(m);
                Vector3d vxyz = minv * v;
                x = vxyz.X;
                y = vxyz.Y;
                z = vxyz.Z;
                return true;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Matrix33 matrix &&
                   EqualityComparer<double[,]>.Default.Equals(_Values, matrix._Values);
        }

        public override int GetHashCode()
        {
            return 229151924 + EqualityComparer<double[,]>.Default.GetHashCode(_Values);
        }
    }

}
