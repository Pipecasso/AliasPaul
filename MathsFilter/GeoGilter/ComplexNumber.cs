using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoFilter
{
    public class ComplexNumber : IEquatable<ComplexNumber>
    {
        public ComplexNumber()
        {
            X = 0;
            Y = 0;
        }

        public ComplexNumber(int x,bool real=true)
        {
            if (real)
            {
                X = x;
            }
            else
            {
                Y = x;
            }
        }

        public ComplexNumber(ComplexNumber other)
        {
            this.X = other.X;
            this.Y = other.Y;
        }

        public ComplexNumber(double a, double b,bool Cartesian = true)
        {
            if (Cartesian)
            {
                X = a;
                Y = b;
            }
            else
            {
                X = a * Math.Cos(b);
                Y = a * Math.Sin(b);
            }
        }

        public double Mod { get => Math.Sqrt(X * X + Y * Y); }
      

        public double Arg { get => Math.Atan2(Y, X); }
      

        public ComplexNumber Conjugate()
        {
            ComplexNumber result = new ComplexNumber(X, -Y);
            return result;
        }

        static public ComplexNumber operator + (ComplexNumber r,ComplexNumber s)
        {
            return new ComplexNumber(r.X + s.X, r.Y + s.Y);
        }

        static public ComplexNumber operator - (ComplexNumber r,ComplexNumber s)
        {
            return new ComplexNumber(r.X - s.X, r.Y - s.Y);
        }

        static public ComplexNumber operator * (ComplexNumber r, ComplexNumber s)
        {
            return new ComplexNumber(r.X * s.X - r.Y * s.Y, r.X * s.Y + r.Y * s.X);
        }

        static public ComplexNumber operator /(ComplexNumber r, double d)
        {
            return new ComplexNumber(r.X / d, r.Y / d);
        }

        static public ComplexNumber operator *(ComplexNumber r, double d)
        {
            return new ComplexNumber(d * r.X, d * r.Y);
        }

        static public ComplexNumber operator / (ComplexNumber r,ComplexNumber s)
        {
            ComplexNumber sconj = s.Conjugate();
            ComplexNumber numerator = r * sconj;
            ComplexNumber denominator = s * sconj;
            return numerator / denominator.Mod;
        }

        static public ComplexNumber operator ^ (ComplexNumber r,int n)
        {
            return new ComplexNumber(Math.Pow(r.Mod, n), n * r.Arg, false);
        }

        public ComplexNumber Sin()
        {
            return new ComplexNumber(Math.Sin(X)*Math.Cosh(Y),Math.Cos(X)*Math.Sinh(Y));
        }

        public ComplexNumber Sinh()
        {
            return new ComplexNumber(Math.Sinh(X) * Math.Cos(Y), Math.Cosh(X) * Math.Sin(Y));
        }

        public ComplexNumber Cos()
        {
            return new ComplexNumber(Math.Cosh(Y) * Math.Cos(X), -Math.Sin(X) * Math.Sinh(Y));
        }
    
        public ComplexNumber Cosh()
        {
            return new ComplexNumber(Math.Cos(Y) * Math.Cosh(X), Math.Sin(Y) * Math.Sinh(X));
        }

        public ComplexNumber Tan()
        {
            return Sin() / Cos();
        }

        public ComplexNumber Tanh()
        {
            return Sinh() / Cosh();
        }



        public double X { get; set; }
        public double Y { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ComplexNumber number &&
                   X == number.X &&
                   Y == number.Y;
        }

        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
        public List<ComplexNumber> Root(int n)
        {
            List<ComplexNumber> result = new List<ComplexNumber>();
            if (n == 0)
            {

            }
            else if (n == 1)
            {
                result.Add(new ComplexNumber(X, Y));
            }
            else
            {
                double mod = Mod;
                double modroot = Math.Pow(mod, 1 / (double)n);
                double arg = Arg;
                for (int i = 0; i < n; i++)
                {
                    double temparg = arg / (double)n + (2 * Math.PI * (double)i) / (double)n;
                    ComplexNumber croot = new ComplexNumber(modroot, temparg, false);
                    result.Add(croot);
                }
            }
            return result;
        }

        public ComplexNumber log()
        {
            return new ComplexNumber(Math.Log(Mod),Arg);
        }

        public bool Equals(ComplexNumber other)
        {
            bool result;
            if (object.ReferenceEquals(other, null))
            {
                result = false;
            }
            else if (object.ReferenceEquals(this, other))
            {
                result = true;
            }
            else if (this.GetType() != other.GetType())
            {
                result = false;
            }
            else
            {
                result = X == other.X && Y == other.Y;
            }
            return result;
        }

        static public bool operator == (ComplexNumber left,ComplexNumber right)
        {
            bool result;
            // Check for null on left side.
            if (Object.ReferenceEquals(left, null))
            {
                if (Object.ReferenceEquals(right, null))
                {
                    // null == null = true.
                    result= true;
                }

                // Only the left side is null.
                result= false;
            }
            else
            {
                result =(left.Equals(right));
            }

            return result;
        }

        static public bool operator !=(ComplexNumber left, ComplexNumber right)
        {
            return !(left == right);
        }
    }
}
