using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pipecasso.Geometry;
using AliasPOD;

namespace KeypointRunner
{
    public class KeypointRunItem
    {
        private ComponentKeypoint _componentKeypoint;
        
        public KeypointRunItem(ComponentKeypoint componentKeypoint)
        {
            _componentKeypoint = componentKeypoint;
        }
        
        public Point3d Get3d()
        {
            double dx, dy, dz;
            _componentKeypoint.Get3DCoordinate(out dx, out dy, out dz);
            Point3d p = new Point3d(dx, dy, dz);
            return p;
        }

        public Point2d Get2d()
        {
            int px, py;
            _componentKeypoint.Get2DCoordinate(out px, out py);
            Point2d p = new Point2d(px, py);
            return p;
        }

        public bool Is3d()
        {
            return _componentKeypoint.Is3DCoordinateValid();
        }

        public bool Is2d()
        {
            return _componentKeypoint.Is2DCoordinateValid();
        }

        public bool InNetwork()
        {
            return _componentKeypoint.Component.InNetwork;
        }

        public string Name
        {
            get 
            { 
                if (InNetwork())
                {
                    return _componentKeypoint.Name;
                }
                else
                {
                    ComponentKeypoint master = _componentKeypoint.Component.GetParentLink(0).Master;
                    return $"Support-{master.Name}";
                }    
            }
        }





    }


    public class CompareToAnchor3d : IComparer<KeypointRunItem>
    {
        private KeypointRunItem _Anchor;
        public CompareToAnchor3d(KeypointRunItem anchor)
        {
            _Anchor = anchor;
        }

        public int Compare(KeypointRunItem x, KeypointRunItem y)
        {
            if (x.Is3d() && y.Is3d())
            {
                double xdist = Point3d.Distance(x.Get3d(), _Anchor.Get3d());
                double ydist = Point3d.Distance(y.Get3d(), _Anchor.Get3d());
                if (xdist < ydist) return -1;
                else if (xdist > ydist) return 1;
                else return 0;
            }
            else if (x.Is3d())
            {
                return -1;
            }
            else if (y.Is3d())
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class CompareToAnchor2d : IComparer<KeypointRunItem>
    {
        private KeypointRunItem _Anchor;

        public CompareToAnchor2d(KeypointRunItem anchor)
        {
            _Anchor = anchor;
        }

        public int Compare(KeypointRunItem x, KeypointRunItem y)
        {
            if (x.Is2d() && y.Is2d())
            {
                double xdist = Point2d.Distance(x.Get2d(), _Anchor.Get2d());
                double ydist = Point2d.Distance(y.Get2d(), _Anchor.Get2d());
                if (xdist < ydist) return -1;
                else if (xdist > ydist) return 1;
                else return 0;
            }
            else if (x.Is2d())
            {
                return -1;
            }
            else if (y.Is2d())
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }


}
