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

        public Camera(Vector3d normal,CubeView cube,double mult,double div,int screenwidth,int screenheight)
        {
            Point3d cubecentre = cube.Center;
            LineCubeIntersection lci = cube.Intersection(cubecentre, normal);
            Line3d intersectline = lci.IntersectionLine();
            double length = intersectline.Length();

            CameraPoint = cubecentre - normal * (mult * length);

            //now project all cubevertices 
            _distance = length / div;
            Normal = normal;
            ChooseOrthogonals();
            _N = CameraPoint + Normal * _distance;

            Dictionary<Point2d,Point3d> VertexMap = new Dictionary<Point2d, Point3d>();

            foreach (Vertex v in cube)
            {
                Point3d vpoint = cube[v];
                Point2d vpointproj = ProjectPoint(vpoint);
                VertexMap.Add(vpointproj, vpoint);
            }


            //2-4 points will make a boundary;
            Point2d top = VertexMap.Keys.Aggregate((acc, cur) => Point2d.Maxy(acc, cur));
            Point2d bottom = VertexMap.Keys.Aggregate((acc, cur) => Point2d.Miny(acc, cur));
            Point2d left = VertexMap.Keys.Aggregate((acc, cur) => Point2d.Minx(acc, cur));
            Point2d right = VertexMap.Keys.Aggregate((acc, cur) => Point2d.Maxx(acc, cur));

            double dheight = CalculateDistanceRequiredForPoint(VertexMap[top], true, screenheight);

         

            /*Point2d a = new Point2d(low.X, high.Y);
            Point2d d = new Point2d(high.X, low.Y);

            Rectangle2d screen = new Rectangle2d(a, d);

            double widthmultipler = screenwidth / screen.Width;
            double heightmultipler = screenheight / screen.Height;
            double actualmult = widthmultipler > heightmultipler ? widthmultipler : heightmultipler;

            _distance *= actualmult;*/

   
        }

        public double CalculateDistanceRequiredForPoint(Point3d p3, bool vertical,double screenlen)
        {

            Vector3d PointToCamera = new Vector3d(CameraPoint, p3);
            double PCLength = PointToCamera.Magnitude();
            PointToCamera.Normalise();

            Vector3d cartesianVector;

            if (vertical)
            {
                cartesianVector = _V1;
            }
            else
            {
                cartesianVector = _V2;
            }

            //consider POINT X the right angle formed by P3 camera point, (as hypotenuse, and the cartesian vector)
            //P3 X is the cartesian vector.
            ///                                       P3
            ////                                       ^     
            ////                                       
            ////     Camera Point ------------------  X

            double PC_CartDP = Vector3d.Dot(PointToCamera, cartesianVector);
            double PX = PCLength * PC_CartDP;
            double angle = Math.Acos(PC_CartDP);
            Point3d X = p3 - cartesianVector * PX;
            Vector3d OX = new Vector3d(CameraPoint, X);
            double OXLen = OX.Magnitude();
            OX.Normalise();
            double check = Vector3d.Dot(OX, cartesianVector);
            //assert check ==0;

            //Consider POINT P2. Its the 3d Point where P3 projects onto the required plane
            //POINT X2 where OX projects onto the required plane
            double ox2len = screenlen * Math.Tan(angle);
            double OXNormalDP = Vector3d.Dot(OX, Normal);
            double len = ox2len * OXNormalDP;

            Point3d N2 = CameraPoint + Normal * len;

            Plane3d PlaneWeWant = new Plane3d(Normal, N2);
            Point3d X2 = CameraPoint + OX * ox2len; ;
            Point3d X2Check = PlaneWeWant.NearestPoint(X2);

            double dist = Point3d.Distance(X2, X2Check);
            //asser ipop = true;





            return len;
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

        public Point3d ProjectPoint3d(Point3d p)
        {
            Vector3d camtopoint = Vector3d.Normalise(p - CameraPoint);
            double theta2 = Vector3d.Dot(camtopoint, Normal); //the acos angle between normal and pointline
            double planedistance = (_distance / theta2);  //the distance from planepoint and camera;
            Point3d P1 = CameraPoint + camtopoint * planedistance; //intersection of the cameraline and the plane
            return P1;
        }

        public Point2d Convert3dTo2d(Point3d P1)
        {
            Point2d pout;
            double maxintasdouble = Convert.ToDouble(int.MaxValue);
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

                pout=  new Point2d(x, y);
            }
            else
            {
                pout = new Point2d(0, 0);
            }
            return pout;
        }

        public Point2d ProjectPoint(Point3d p)
        {
         
            Point3d P1 = ProjectPoint3d(p);
            Point2d p2 = Convert3dTo2d(P1);
            return p2;
        }

        public Point3d N { get => _N; set { _N = value; } }

        public double distance { get => _distance; set { _distance = value; } }

    }
}
