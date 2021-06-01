using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AliasGeometry
{
    public enum NorthArrow
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    };

    public enum Face
    {
        None,
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back
    };

    public enum LineCubeIntersection
    {
        Unset,
        Miss,
        Touch,
        PassThrough,
        Surface
    };


    public class Vertex 
    {
        private Face _frontback;
        private Face _updown;
        private Face _leftright;

        public Face Frontback { get => _frontback;}
        public Face Updown { get => _updown; }
        public Face Leftright { get => _leftright; }

        public Vertex(Face frontback,Face updown,Face leftright)
        {
            _frontback = frontback;
            _updown = updown;
            _leftright = leftright;
        }

        public bool IsValid()
        {
            return (_frontback == Face.Front || _frontback == Face.Back) && (_leftright == Face.Left || _leftright == Face.Right) && (_updown == Face.Top || _updown == Face.Bottom);
        }

       

        static private Face Opposite(Face f)
        {
            Face fout = Face.None;
            switch (f)
            {
                case Face.Top: fout = Face.Bottom; break;
                case Face.Bottom: fout = Face.Top; break;
                case Face.Front:fout = Face.Back; break;
                case Face.Back:fout = Face.Front;break;
                case Face.Left:fout = Face.Right;break;
                case Face.Right:fout = Face.Left;break;
            }
            return fout;
        }

        static public Vertex Opposite(Vertex v)
        {
            return new Vertex(Opposite(v._frontback), Opposite(v._updown), Opposite(v._leftright));
        }

        static public Vertex OppositeUD(Vertex v)
        {
            return new Vertex(v._frontback, Opposite(v._updown), v._leftright);
        }

        static public Vertex OppositeFB(Vertex v)
        {
            return new Vertex(Opposite(v._frontback), v._updown, v._leftright);
        }

        static public Vertex OppositeLR(Vertex v)
        {
            return new Vertex(v._frontback, v._updown, Opposite(v._leftright));
        }
    
    }

    public class VertexCompare : IEqualityComparer<Vertex>
    { 
        public bool Equals(Vertex l,Vertex r)
        {
            return (l.Frontback == r.Frontback && l.Leftright == r.Leftright && l.Updown == r.Updown);
        }

        public int GetHashCode(Vertex v)
        {
            int hash = 17;
            hash = hash + 23 * v.Leftright.GetHashCode();
            hash = hash + 23 * v.Updown.GetHashCode();
            hash = hash + 23 * v.Frontback.GetHashCode();
            return hash;
        }
    
    
    }



    public class CubeView
    {
        private Dictionary<Vertex, Point3d> _Vertices;
        private Point3d _center;
        private Vertex _ftl;
        private Vertex _fbl;
        private Vertex _ftr;
        private Vertex _fbr;
        private Vertex _btl;
        private Vertex _btr;
        private Vertex _bbr;
        private Vertex _bbl;

        const int CAM_FROM_NORM = 10;

    




        public CubeView(List<Point3d> points,NorthArrow na = NorthArrow.TopLeft)
        {
            double lowestX = double.MaxValue;
            double lowestY = double.MaxValue;
            double lowestZ = double.MaxValue;
            double highestX = double.MinValue;
            double highestY = double.MinValue;
            double highestZ = double.MinValue;

            foreach (var P in points)
            {
                if (P.X >= highestX) { highestX = P.X; }
                if (P.Y >= highestY) { highestY = P.Y; }
                if (P.Z >= highestZ) { highestZ = P.Z; }

                if (P.X <= lowestX) { lowestX = P.X; }
                if (P.Y <= lowestY) { lowestY = P.Y; }
                if (P.Z <= lowestZ) { lowestZ = P.Z; }
            }
            InitialiseCube(highestX, lowestX,highestY,lowestY,highestZ,lowestZ,na);
        }


        private void InitialiseCube(double highx, double lowx, double highy, double lowy, double highz, double lowz, NorthArrow na)
        {

            Action<Vertex,Point3d,Point3d> SetCrossPlane = (v,p,o) => 
            {
                _Vertices[v].Set(p.X, p.Y,p.Z);
                Vertex Oppv = Vertex.Opposite(v);
                _Vertices[Oppv].Set(o.X, o.Y, o.Z);
                Vertex Upv = Vertex.OppositeUD(v);
                _Vertices[Upv].Set(p.X, p.Y, o.Z);
                Vertex OppUpv = Vertex.Opposite(Upv);
                _Vertices[OppUpv].Set(o.X, o.Y, p.Z);

            };
            
            
            _Vertices = new Dictionary<Vertex, Point3d>(new VertexCompare());
            _ftl = new Vertex(Face.Front, Face.Top, Face.Left);
            _ftr = new Vertex(Face.Front, Face.Top, Face.Right);
            _fbl = new Vertex(Face.Front, Face.Bottom, Face.Left);
            _fbr = new Vertex(Face.Front, Face.Bottom, Face.Right);
            _bbr = Vertex.Opposite(_ftl);
            _bbl = Vertex.Opposite(_ftr);
            _btr = Vertex.Opposite(_fbl);
            _btl = Vertex.Opposite(_fbr);
            _Vertices.Add(_ftl, new Point3d());
            _Vertices.Add(_ftr, new Point3d());
            _Vertices.Add(_fbl, new Point3d());
            _Vertices.Add(_fbr, new Point3d());
            _Vertices.Add(_bbr, new Point3d());
            _Vertices.Add(_bbl, new Point3d());
            _Vertices.Add(_btr, new Point3d());
            _Vertices.Add(_btl, new Point3d());



            Vertex Anchor = null;
            Point3d AnchorPoint = new Point3d(lowx, lowy, lowz);
            Point3d AnchorOppPoint = new Point3d(highx, highy, highz);
            Point3d AnchorLrPoint = null;
            Point3d AnchorOppLrPoint = null;
            switch (na)
            {
                case NorthArrow.TopLeft: 
                    Anchor = _fbr;
                   
                    AnchorLrPoint = new Point3d(lowx,highy,lowz);
                    AnchorOppLrPoint = new Point3d(highx, lowy, highz);
                    break;
                case NorthArrow.TopRight:
                    Anchor = _fbl;
                    
                    AnchorLrPoint = new Point3d(highx, lowy, lowz);
                    AnchorOppLrPoint = new Point3d(lowx, highy, highz);
                    break;
                case NorthArrow.BottomRight:
                    Anchor = _bbl;
                    AnchorLrPoint = new Point3d(lowx, highy, lowz);
                    AnchorOppLrPoint = new Point3d(highx, lowy, highz);
                    break;
                case NorthArrow.BottomLeft:
                    Anchor = _bbr;
                    AnchorLrPoint = new Point3d(highx, lowy, lowz);
                    AnchorOppLrPoint = new Point3d(lowx, highy, highz);
                    break;
            }
            SetCrossPlane(Anchor, AnchorPoint, AnchorOppPoint);
            Vertex AnchorAdj = Vertex.OppositeLR(Anchor);
            SetCrossPlane(AnchorAdj, AnchorLrPoint, AnchorOppLrPoint); //it fucks up here.

            _center = new Point3d((lowx + highx) / 2, (lowy + highy) / 2, (lowz + highz) / 2);
        }

        /*
        //FRONT = ABCD
        public Point3d getCameraFrontView()
        {
            double horizontalDistance = Point3d.Distance(_ftl, _ftr) / 2;
            double verticalDistance = Point3d.Distance(_ftl, _fbl) / 2;
            Point3d FrontCentre = new Point3d(_ftl.X + horizontalDistance, _ftl.Y - verticalDistance, _ftl.Z + CAM_FROM_NORM);

            return FrontCentre;
        }




        //BACK = EFGH
        public Point3d getCameraBackView()
        {
            double horizontalDistance = Point3d.Distance(_btl, _btr) / 2;
            double verticalDistance = Point3d.Distance(_btl, _bbl) / 2;
            Point3d BackCentre = new Point3d(_btl.X + horizontalDistance, _btl.Y - verticalDistance, _btl.Z - CAM_FROM_NORM);
            return BackCentre;

        }



        //TOP
        public Point3d getCameraTopView()
        {
            double horizontalDistance = Point3d.Distance(_btl, _btr) / 2;
            double verticalDistance = Point3d.Distance(_btl, _ftl) / 2;
            return new Point3d(_btl.X + horizontalDistance, _btl.Y + CAM_FROM_NORM, _btl.Z + verticalDistance);
        }



        //BOTTOM
        public Point3d getCameraBottomView()
        {
            double horizontalDistance = Point3d.Distance(_bbl, _bbr) / 2;
            double verticalDistance = Point3d.Distance(_bbl, _fbl) / 2;
            return new Point3d(_bbl.X + horizontalDistance, _bbl.Y - CAM_FROM_NORM, _bbl.Z + verticalDistance);
        }


        //RIGHTSIDE
        public Point3d getCameraRightSideView()
        {
            double horizontalDistance = Point3d.Distance(_ftr, _btr) / 2;
            double verticalDistance = Point3d.Distance(_ftr, _fbr) / 2;
            return new Point3d(_ftr.X + CAM_FROM_NORM, _ftr.Y - verticalDistance, _ftr.Z - horizontalDistance);
        }

  

        //LEFTSIDE
        public Point3d getCameraLeftSideView()
        {
            double horizontalDistance = Point3d.Distance(_ftl, _btl) / 2;
            double verticalDistance = Point3d.Distance(_ftl, _fbl) / 2;
            return new Point3d(_ftl.X - CAM_FROM_NORM, _ftl.Y - verticalDistance, _ftl.Z - horizontalDistance);
        }*/

        public Point3d Center
        {
            get
            {
                return _center;
            }
        }

        public string CenterDisplay
        {
            get
            {
                return $"({Center.X.ToString()},{Center.Y.ToString()},{Center.Z.ToString()})";
            }
        }

        public string Dimensions
        {
            get
            {
                return $"{string.Format("{0:0.###}",LeftRightDistance())} x {string.Format("{0:0.###}",FrontBackDistance())} x {string.Format("{0:0.###}",TopBottomDistance())}";
            }
        }



        public double FrontBackDistance()
        {
            return Point3d.Distance(FrontTopLeft, BackTopLeft);
        }

        public double TopBottomDistance()
        {
            return Point3d.Distance(FrontTopLeft, FrontBottomLeft);
        }

        public double LeftRightDistance()
        {
            return Point3d.Distance(FrontTopLeft, FrontTopRight);
        }

        public double FTLBBRDistance()
        {
            return Point3d.Distance(FrontTopLeft, BackBottomRight);
        }

        public double FTRBBLDistance()
        {
            return Point3d.Distance(FrontTopRight, BackBottomLeft);
        }

        public double FBRBTLDistance()
        {
            return Point3d.Distance(FrontBottomRight, BackTopLeft);
        }



        public Point3d FrontTopLeft
        {
            get
            {
                return _Vertices[_ftl];
            }
        }

        public Point3d BackBottomRight
        {
            get
            {
                return _Vertices[_bbr];
            }
        }

        public Point3d FrontTopRight
        {
            get
            {
                return _Vertices[_ftr];
            }
        }

        public Point3d BackBottomLeft
        {
            get
            {
                return _Vertices[_bbl];
            }
        }

        public Point3d FrontBottomLeft
        {
            get
            {
                return _Vertices[_fbl];
            }
        
        }

        public Point3d FrontBottomRight
        {
            get
            {
                return _Vertices[_fbr];
            }
        }

        public Point3d BackTopLeft
        {
            get
            {
                return _Vertices[_btl];
            }
        }

        public Point3d BackTopRight
        {
            get
            {
                return _Vertices[_btr];
            }
        }

        public Point3d this[Vertex v]
        {
            get
            {
                if (v.IsValid())
                {
                    return _Vertices[v];
                }
                else
                {
                    return null;
                }    

            }
        }


        public string Name { get; set; }

        public LineCubeIntersection Intersection(Point3d point,Vector3d vector, out Point3d p1,out Point3d p2)
        {
            p1 = null;
            p2 = null;
            Dictionary<Face,BoundedPlane3d> sixPlanes = SixPlanes();
            int hitcount = 0;
            LineCubeIntersection lineCubeIntersection = LineCubeIntersection.Unset;
            foreach (KeyValuePair<Face,BoundedPlane3d> kvp in sixPlanes)
            {
                Point3d p = new Point3d();
                //this is a bit inefficiant as the IsPointOnPlane is also called durining intersection;
                BoundedPlane3d boundedPlane3 = kvp.Value;
                bool intersectsbase = boundedPlane3.Intersection(point, vector, ref p);

                if (intersectsbase)
                {
                    if (boundedPlane3.IsPointOnPlane(p))
                    {
                        if (hitcount == 0)
                        {
                            p1 = p;
                            hitcount++;
                        }
                        else
                        {
                            p2 = p;
                            hitcount++;
                        }
                        if (hitcount == 2) break;
                    }
                }
                else
                {
                    lineCubeIntersection = LineCubeIntersection.Surface;
                    break;
                }
            }

            if (lineCubeIntersection != LineCubeIntersection.Surface)
            {
                switch (hitcount)
                {
                    case 0: lineCubeIntersection = LineCubeIntersection.Miss; break;
                    case 1: lineCubeIntersection = LineCubeIntersection.Touch; break;
                    case 2: lineCubeIntersection = LineCubeIntersection.PassThrough; break;

                }
            }


            return lineCubeIntersection;
        }

        public Dictionary<Face, BoundedPlane3d> SixPlanes()
        {
            Dictionary<Face, BoundedPlane3d> sixPlanes = new Dictionary<Face, BoundedPlane3d>();
            sixPlanes.Add(Face.Front, new BoundedPlane3d(FrontTopLeft, FrontBottomRight, FrontTopRight, FrontBottomLeft));
            sixPlanes.Add(Face.Back, new BoundedPlane3d(BackTopLeft, BackBottomRight, BackTopRight, BackBottomLeft));
            sixPlanes.Add(Face.Top, new BoundedPlane3d(BackTopLeft, FrontTopRight, BackTopRight, FrontTopLeft));
            sixPlanes.Add(Face.Bottom, new BoundedPlane3d(BackBottomLeft, FrontBottomRight, BackBottomRight, FrontBottomLeft));
            sixPlanes.Add(Face.Left, new BoundedPlane3d(BackTopLeft, FrontBottomLeft, FrontTopLeft, BackBottomLeft));
            sixPlanes.Add(Face.Right, new BoundedPlane3d(BackTopRight, FrontBottomRight, FrontTopRight, BackBottomRight));
            return sixPlanes;

        }
        






    }
}
    
