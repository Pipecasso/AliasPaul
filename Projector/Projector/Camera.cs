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
                if (!VertexMap.ContainsKey(vpointproj))
                {
                    VertexMap.Add(vpointproj, vpoint);
                }
            }


            //2-4 points will make a boundary;
            Point2d top = VertexMap.Keys.Aggregate((acc, cur) => Point2d.Maxy(acc, cur));
            Point2d bottom = VertexMap.Keys.Aggregate((acc, cur) => Point2d.Miny(acc, cur));
            Point2d left = VertexMap.Keys.Aggregate((acc, cur) => Point2d.Minx(acc, cur));
            Point2d right = VertexMap.Keys.Aggregate((acc, cur) => Point2d.Maxx(acc, cur));

            List<double> Distances = new List<double>();
            Distances.Add(CalculateDistanceRequiredForPoint(VertexMap[top], true, screenheight/2));
            Distances.Add(CalculateDistanceRequiredForPoint(VertexMap[bottom], true, screenheight/2));
            Distances.Add(CalculateDistanceRequiredForPoint(VertexMap[left], false, screenwidth/2));
            Distances.Add(CalculateDistanceRequiredForPoint(VertexMap[right], false, screenwidth/2));
            _distance = Distances.Min();

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
            if (angle > Math.PI /2)
            {
                angle = Math.PI - angle;
            }
            
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

        private Cone2d ProjectCone(Cone3d conical)
        {
            /*Point2d pc1 = ProjectPoint(conical.circleStart.Center);
            Point2d pc2 = ProjectPoint(conical.circleStart.Polar(0));
            Point2d pc3 = ProjectPoint(conical.circleStart.Polar((float)Math.PI/2));

            double rad1 = Point2d.Distance(pc1, pc2);
            double rad2 = Point2d.Distance(pc1, pc3);

            Ellipse2d e1 = new Ellipse2d(pc1, rad2, rad1);

            pc1 = ProjectPoint(conical.circleEnd.Center);
            pc2 = ProjectPoint(conical.circleEnd.Polar(0));
            pc3 = ProjectPoint(conical.circleEnd.Polar((float)Math.PI / 2));

             rad1 = Point2d.Distance(pc1, pc2);
             rad2 = Point2d.Distance(pc1, pc3);

            Ellipse2d e2 = new Ellipse2d(pc1, rad2, rad1);

            Cone2d coneit = new Cone2d(e1, e2);
            return coneit;*/

            Circle3dPointByPoint start = conical.circleStart;
            Ellipse2dPointByPoint e1 = ProjectCircle(start);
            e1.DerriveRadii();
            Circle3dPointByPoint end = conical.circleEnd;
            Ellipse2dPointByPoint e2 = ProjectCircle(end);
            e2.DerriveRadii();
            Cone2d coneit = new Cone2d(e1, e2);
            return coneit;
        }



        private Ellipse2dPointByPoint ProjectCircle(Circle3dPointByPoint start)
        {

            Point3d pCenter = start.Center;
            Point2d pc1 = ProjectPoint(pCenter);
            Ellipse2dPointByPoint e = new Ellipse2dPointByPoint(pc1);
            for (int i = 0; i < 360; i++)
            {
                Point3d cpoint = start[i];
                Point2d epoint = ProjectPoint(cpoint);
                e[i] = epoint;
            }
            return e;

        }
        private Line2d ProjectLine(Line3d line)
        {
            Point2d p2d1 = ProjectPoint(line.P);
            Point2d p2d2 = ProjectPoint(line.Q);

            return new Line2d(p2d1, p2d2);
        }
        /*
        Cone3d MakeCorrectedCone(AliasPOD3D.Cone c, ProjectablePod pp, string uid)
        {
            double dx, dy, dz;
            c.GetStart(out dx, out dy, out dz);
            Point3d ptStart = new Point3d(dx, dy, dz);
            System.Diagnostics.Debug.WriteLine("Circle Start {0} {1} {2}", dx, dy, dz);
            c.GetEnd(out dx, out dy, out dz);
            Point3d ptEnd = new Point3d(dx, dy, dz);
            System.Diagnostics.Debug.WriteLine("Circle End {0} {1} {2}", dx, dy, dz);
            Point3d ptMid = Point3d.MidPoint(ptStart, ptEnd);
            System.Diagnostics.Debug.WriteLine("Circle Mid {0} {1} {2}", ptMid.X, ptMid.Y, ptMid.Z);
            double conelength = Point3d.Distance(ptStart, ptEnd);
            Vector3d vConeLine = null;
            Vector3d vExistingConeLine = new Vector3d(ptStart, ptEnd);
            vExistingConeLine.Normalise();

            bool FoundConeDir = false;
            if (pp.UidMap.ContainsKey(uid))
            {
                AliasPOD.Component cpent = pp.UidMap[uid];
                if (cpent.Material.Configuration == AliasPOD.eConfigurationType.eCTInline)
                {
                    AliasPOD.ComponentLeg cl = cpent.Legs.Item(0);
                    if (cl.IsDirectionValid() == true)
                    {
                        cl.GetDirection(out dx, out dy, out dz);
                        vConeLine = new Vector3d(dx, dy, dz);
                        FoundConeDir = true;
                    }
                }
            }
            if (!FoundConeDir)
            {
                vConeLine = vExistingConeLine;
            }

            double check = Vector3d.Dot(vConeLine, vExistingConeLine);

            System.Diagnostics.Debug.WriteLine("New Cone Line {0},{1},{2}", vConeLine.X, vConeLine.Y, vConeLine.Z);
            Cone3d conetogo = new Cone3d(ptMid, conelength, vConeLine, c.StartDiameter, c.EndDiameter);
            System.Diagnostics.Debug.WriteLine("Circle Start New {0} {1} {2}", conetogo.circleStart.Center.X, conetogo.circleStart.Center.Y, conetogo.circleStart.Center.Z);
            System.Diagnostics.Debug.WriteLine("Circle Start End {0} {1} {2}", conetogo.circleEnd.Center.X, conetogo.circleEnd.Center.Y, conetogo.circleEnd.Center.Z);
            return conetogo;
        }*/

        public Point3d N { get => _N; set { _N = value; } }

        public double distance { get => _distance; set { _distance = value; } }

    }
}
