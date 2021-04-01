using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Diagnostics;

namespace AliasGeometry
{


    public class Cone3d
    {
        private Circle3d _circleStart;
        private Circle3d _circleEnd;



        public Cone3d(Point3d mid, double length, Vector3d centerline, double r1, double r2)
        {
            Vector3d vacross, vup;
            Point3d top = mid + centerline * (length / 2);
            Point3d bottom = mid - centerline * (length / 2);
            UpAndAcross(centerline, out vup, out vacross);
            _circleStart = new Circle3d(top, r1, centerline,vup,vacross);
            _circleEnd = new Circle3d(bottom, r2, centerline,vup,vacross);
        }

        private void UpAndAcross(Vector3d vNCenterLine,out Vector3d vup,out Vector3d vacross)
        {
            Vector3d vertical = new Vector3d(0, 0, 1);
            double fabsDot = Math.Abs(Vector3d.Dot(vNCenterLine, vertical));
            if (Math.Abs(fabsDot - 1) < 0.0001)
            {
                vup = new Vector3d(0, 1, 0);
                vacross = new Vector3d(1, 0, 0);
            }
            else
            {
                vacross = Vector3d.Normalise(Vector3d.CrossProduct(vNCenterLine, vertical));
                vup = Vector3d.Normalise(Vector3d.CrossProduct(vacross, vNCenterLine));
            }


        }

        public Circle3d circleStart
        {
            get
            {
                return _circleStart;
            }
        }

        public Circle3d circleEnd
        {
            get
            {
                return _circleEnd;
            }
        }

        public double Height()
        {
            return Point3d.Distance(_circleStart.Center, _circleEnd.Center);
        }

        public Vector3d CenterLine()
        {
            Vector3d vCenter = _circleEnd.Center - _circleStart.Center;
            return Vector3d.Normalise(vCenter);
        }
    }
}
