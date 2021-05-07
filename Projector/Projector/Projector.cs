using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliasGeometry;

namespace Projector
{
    public class Camera
    {
       public Point3d CameraPoint { get; set; }
        public Vector3d Normal { get; set; }
        public Vector3d _V1;
        public Vector3d _V2;

        double _distance;

        Point3d _N; //intersection of normal and plane

        public Camera() { }

        public Camera(Point3d campoint, double distance, Vector3d normal)
        {
            CameraPoint = campoint;
            _distance = distance;
            Normal = normal;
            ChooseOrthogonals();
            _N = CameraPoint + Normal * _distance;
        }

        public Vector3d V1 { get => _V1; set { _V1 = value; } }
        public Vector3d V2 { get => _V2; set { _V2 = value; } }

        void ChooseOrthogonals()
        {
            //now pick to 3d vectors to represent the axis on the plane
            Vector3d vertical = new Vector3d(0, 0, 1);
            double fabsDot = Math.Abs(Vector3d.Dot(Normal, vertical));
            if (Math.Abs(fabsDot) < double.Epsilon)
            {
                _V1 = new Vector3d(0, 1, 0);
                _V2 = new Vector3d(1, 0, 0);
            }
            else
            {
                _V2 = Vector3d.Normalise(Vector3d.CrossProduct(Normal, vertical));
                _V1 = Vector3d.Normalise(Vector3d.CrossProduct(_V2, Normal));
            }

        }

        public Point2d ProjectPoint(Point3d p)
        {
            double maxintasdouble = Convert.ToDouble(int.MaxValue);
            Vector3d camtopoint = Vector3d.Normalise(p - CameraPoint);
            double theta2 = Vector3d.Dot(camtopoint, Normal); //the acos angle between normal and pointline
            double planedistance = (_distance / theta2);  //the distance from planepoint and camera;
            Point3d P1 = CameraPoint + camtopoint * planedistance; //intersection of the cameraline and the plane
            Vector3d NP1 = P1 - _N;
            double hypdist = NP1.Magnitude();


            if (Math.Abs(hypdist) > 0.00001)
            {

                Vector3d NP1N = NP1.Magnitude() == 0 ? NP1 : Vector3d.Normalise(NP1);
                double dottohorizontal = Vector3d.Dot(NP1N, _V2);
                double dottovertical = Vector3d.Dot(NP1N, _V1);

                double dx = Math.Floor(hypdist * dottohorizontal + 0.5);
                double dy = Math.Floor(hypdist * dottovertical + 0.5);

                int x = (Math.Abs(dx) > maxintasdouble) ? x = Math.Sign(dx) * Int32.MaxValue : Convert.ToInt32(dx);
                int y = (Math.Abs(dy) > maxintasdouble) ? x = Math.Sign(dy) * Int32.MaxValue : Convert.ToInt32(dy);

                return new Point2d(x, y);
            }
            else
            {
                return new Point2d(0, 0);
            }
        }

        public Point3d N { get => _N; set { _N = value; } }

        public double distance { get => _distance; set { _distance = value; } }

    }
}
