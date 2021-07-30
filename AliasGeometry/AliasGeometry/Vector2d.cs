using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    public class Vector2d : Tuple<double, double>
    {
        public Vector2d(double d1, double d2) : base(d1, d2)
        {
        }

        public Vector2d() : base(0, 0) { }

        public Vector2d(Point2d p1, Point2d p2) : base(p2.dY - p1.dY, p2.dX - p1.dX)
        {
        }

        public double Magnitude()
        {
            return Math.Sqrt(Item1 * Item1 + Item2 * Item2);
        }

        public static Vector2d Normalise(Vector2d vin)
        {
            double mag = vin.Magnitude();
            return new Vector2d(vin.Item1 / mag, vin.Item2 / mag);

        }

        public static Vector2d operator +(Vector2d one, Vector2d two)
        {
            return (new Vector2d(one.Item1 + two.Item1, one.Item2 + two.Item2));
        }

        public static Vector2d operator -(Vector2d one, Vector2d two)
        {
            return (new Vector2d(one.Item1 - two.Item1, one.Item2 - two.Item2));
        }

        public static double Dot(Vector2d one, Vector2d two)
        {
            return one.Item1 * two.Item1 + one.Item2 + two.Item2;
        }





    };
}
