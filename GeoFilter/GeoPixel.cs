using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoFilter
{
    public class GeoPixel : IEquatable<GeoPixel>
    {
        private int _x;
        private int _y;

        public GeoPixel(int x,int y)
        {
            _x = x;
            _y = y;
        }

        public int x
        {
            get
            {
                return _x;
            }
        }

        public int y
        {
            get
            {
                return _y;
            }
        }

        #region equality
 

        static public bool operator == (GeoPixel left, GeoPixel right)
        {
            // Check for null on left side.
            if (Object.ReferenceEquals(left, null))
            {
                if (Object.ReferenceEquals(right, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }

            return left.Equals(right);
        }


        static public bool operator != (GeoPixel left, GeoPixel right)
        {
            return !(left == right);

        }

        private int ShiftAndWrap(int value, int positions)
        {
            positions = positions & 0x1F;

            // Save the existing bit pattern, but interpret it as an unsigned integer.
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            // Preserve the bits to be discarded.
            uint wrapped = number >> (32 - positions);
            // Shift and wrap the discarded bits.
            return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as GeoPixel);
        }

        public  bool Equals(GeoPixel other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != other.GetType())
            {
                return false;
            }

            GeoPixel p = (GeoPixel)other;
            return _x == other._x && _y == other._y;
        }

        public override int GetHashCode()
        {
            return ShiftAndWrap(_x.GetHashCode(), 2) ^ _y.GetHashCode();
        }

        #endregion
    }
}
