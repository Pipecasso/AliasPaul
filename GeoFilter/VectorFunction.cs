using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace GeoFilter
{
    public class VectorFunctions
    { 
           public static Func< List<ColourVector>, ColourVector>  Average = (colours) =>
             {
                 ColourVector start = new ColourVector(0, 0, 0);
                 foreach (ColourVector cv in colours)
                 {
                     start.Red = start.Red + cv.Red;
                     start.Green = start.Green + cv.Green;
                     start.Blue = start.Blue + cv.Blue;
                 }

                 start.Red = start.Red / colours.Count;
                 start.Green = start.Green / colours.Count;
                 start.Blue = start.Blue / colours.Count;

                 return start;
             };


        public static Func<List<ColourVector>, ColourVector> Total = (colours) =>
        {
            ColourVector start = new ColourVector(0, 0, 0);
            foreach (ColourVector cv in colours)
            {
                start.Red = start.Red + cv.Red;
                start.Green = start.Green + cv.Green;
                start.Blue = start.Blue + cv.Blue;
            }
            return start;
        };


        public static Func<List<ColourVector>, ColourVector> Visage = (colours) =>
        {

            ColourVector fadetogrey = new ColourVector(128, 128, 128);
            ColourVector visage = null;
            double dDistance = double.MaxValue;
            foreach (ColourVector cv in colours)
            {
                DenseVector dv = (cv - fadetogrey);
                double dt = dv.L2Norm();
                if (dt < dDistance)
                {
                    dDistance = dt;
                    visage = cv;
                }
            }

            return visage;
        };

        public static Func<List<ColourVector>, ColourVector> VisageReverse = (colours) =>
        {

            ColourVector fadetogrey = new ColourVector(128, 128, 128);
            ColourVector visage = null;
            double dDistance = -1;
            foreach (ColourVector cv in colours)
            {
                DenseVector dv = (cv - fadetogrey);
                double dt = dv.L2Norm();
                if (dt > dDistance)
                {
                    dDistance = dt;
                    visage = cv;
                }
            }

            return visage;
        };

        public static Func<List<ColourVector>, ColourVector> Cross = (colours) =>
        {
            ColourVector one = null;
            foreach (ColourVector cv in colours)
            {
                if (one == null)
                {
                    one = cv;
                }
                else
                {
                    one = ColourVector.CrossProduct(one, cv);
                }
            }
            return one;
        };



    }
}

