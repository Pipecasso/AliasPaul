using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Drawing;
using System.Diagnostics;

namespace GeoFilter
{
    public enum OutOfBounds
    {
        Reject,
        Stop,
        Rollover,
        Bounce,
        Normalise
    }

    public class ColourVector : DenseVector
    {

        public ColourVector(int val,OutOfBounds oob) : base(new double[3])
        {
            const int uppoerbound = 16777216;
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
                            output = uppoerbound - 1;
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

            int b = output / 65536;
            int rg = output % 65536;
            int g = rg / 256;
            int r = rg % 256;

            this[0] = r;
            this[1] = g;
            this[2] = b;

        }

        public ColourVector(Color c) : base(new double[3] { c.R,c.G,c.B})
        {
        }

        public ColourVector(double dx,double dy,double dz) : base(new double[3] { dx, dy, dz })
        {
        }

        public ColourVector(Vector v) : base(new double[3] { v[0], v[1], v[2] })
        {

        }

        public ColourVector(Vector<double> v) : base(new double[3] { v[0], v[1], v[2] })
        {

        }
        #region colorconvert
        public Color GetColour()
        {
            int r = Convert.ToInt32(Math.Floor(this[0] + 0.5));
            int g = Convert.ToInt32(Math.Floor(this[1] + 0.5));
            int b = Convert.ToInt32(Math.Floor(this[2] + 0.5));
            return Color.FromArgb(r, g, b);

        }

        public bool ValidColour(ref Color rgb)
        {
            int r = Convert.ToInt32(Math.Floor(this[0] + 0.5));
            int g = Convert.ToInt32(Math.Floor(this[1] + 0.5));
            int b = Convert.ToInt32(Math.Floor(this[2] + 0.5));

            bool bValid = r >= 0 && r < 256 && g >= 0 && g < 256 && b >= 0 && b < 256;
            if (bValid)
            {
                rgb = Color.FromArgb(r, g, b);
            }
            return bValid;
        }

        public Color RolloverColour()
        {
            Func<int, int> Rollover = (i) =>
             {
                 int iout = i;
                 if (i > 255)
                 {
                     iout = i % 256;
                 }
                 else if (i < 0)
                 {
                     int below = Math.Abs(i) % 256;
                     iout = 255 - below;
                 }
                 Debug.Assert(iout >= 0 && iout < 256);
                 return iout;

             };

            int r = Convert.ToInt32(Math.Floor(this[0] + 0.5));
            int g = Convert.ToInt32(Math.Floor(this[1] + 0.5));
            int b = Convert.ToInt32(Math.Floor(this[2] + 0.5));

            int ir = Rollover(r);
            int ig = Rollover(g);
            int ib = Rollover(b);

            return Color.FromArgb(ir, ig, ib);
            

        }

        public Color NormalisedColour()
        {
            DenseVector dv = (DenseVector)this.Normalize(2);
            dv *= 255;
            ColourVector togo = new ColourVector(dv);
            return togo.GetColour();


        }
        #endregion

        public double Red
        {
            get
            {
                return this[0];
            }
            set
            {
                this[0] = value;

            }
        }

        public double Green
        {
            get
            {
                return this[1];
            }
            set
            {
                this[1] = value;

            }
        }

        public double Blue
        { 
            get
            {
                return this[2];
            }
            set
            {
                this[2] = value;
            }
        }

        public int IRed
        {
            get => Convert.ToInt32(Math.Floor(this[0] + 0.5));
        }

        public int IGreen
        {
            get => Convert.ToInt32(Math.Floor(this[1] + 0.5));
        }

        public int IBlue
        {
            get => Convert.ToInt32(Math.Floor(this[2] + 0.5));
        }

        public int SingleRgb
        {
            get => IRed + IGreen * 256 + IBlue * 65536;
        }

        public double FuncMe(Func<double,double,double,double> fme)
        {
            return fme(this[0], this[1], this[2]);

        }

        static public ColourVector CrossProduct(ColourVector left, ColourVector right)
        {
            double r = left[1] * right[2] - left[2] * right[1];
            double g = -left[0] * right[2] + left[2] * right[0];
            double b = left[0] * right[1] - left[1] * right[0];

            return new ColourVector(r, g, b);
        }
        

        public bool ColourBattle (ColourVector other,Func<ColourVector,ColourVector,bool> funky)
        {
            return funky(this, other);
        }

        
 





    }
}
