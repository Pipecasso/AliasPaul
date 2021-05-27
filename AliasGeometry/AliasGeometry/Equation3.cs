using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    public class Equation
    {
        public Equation(double x, double y, double z, double c)
        {
            X = x;
            Y = y;
            Z = z;
            C = c;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double  Z { get; set; }
        public double C { get; set; }


    
    }

    
    public class Equation3
    {
        public Equation3(Equation e1, Equation e2, Equation e3)
        {
            this.e1 = e1;
            this.e2 = e2;
            this.e3 = e3;
        }

        public Equation e1 { get; set; }
        public Equation e2 { get; set; }
        public Equation e3 { get; set; }


        public bool Solve(ref double x,ref double y,ref double z)
        {

            bool bRet = false;
            Matrix33 D = new Matrix33();
            D[0, 0] = e1.X;
            D[1, 0] = e1.Y;
            D[2, 0] = e1.Z;
            D[0, 1] = e2.X;
            D[1, 1] = e2.Y;
            D[2, 1] = e2.Z;
            D[0, 2] = e3.X;
            D[1, 2] = e3.Y;
            D[2, 2] = e3.Z;
            Vector3d C = new Vector3d(e1.C, e2.C, e3.C);
            double DDet = D.Determinant;

            if (Math.Abs(DDet) > 0)
            {
                Matrix33 dx = new Matrix33(C, D.column(1), D.column(2),false);
                Matrix33 dy = new Matrix33(D.column(0), C, D.column(2), false);
                Matrix33 dz = new Matrix33(D.column(0), D.column(1), C,false);
           
                x = dx.Determinant / DDet;
                y = dy.Determinant / DDet;
                z = dz.Determinant / DDet;

                bRet = true;

            }
            return bRet;
        }
    
    }
}
