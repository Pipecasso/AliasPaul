using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    public class BoundedPlane3d : Plane3d
    {
       

        private Point3d _TopLeft;
        private Point3d _BottomRight;
        private Point3d _TopRight;
        private Point3d _BottomLeft;

        public BoundedPlane3d(Vector3d v,Point3d topleft,Point3d bottomright,Point3d topright,Point3d bottomleft) : base(v,Point3d.MidPoint(topleft,bottomright))
        {
            _TopLeft = topleft;
            _BottomRight = bottomright;
            _TopRight = topleft;
            _BottomLeft = bottomleft;
        }

        public new bool IsPointOnPlane(Point3d P)
        {
            bool ret = false;
            if (base.IsPointOnPlane(P))
            {
                Line3d planeline = new Line3d(_P, P);
         
            }
            return ret;
        }


        public Line3d Top
        {
            get
            {
                return new Line3d(_TopLeft, _TopRight);
            }
        }

        public Line3d Bottom
        {
            get
            {
                return new Line3d(_BottomLeft, _BottomRight);
            }
        
        }

        public Line3d Left
        { 
            get
            {
                return new Line3d(_TopLeft, _BottomLeft);
            }
        
        }

        public Line3d Right
        { 
            get
            {
                return new Line3d(_TopRight, _BottomRight);
            }
        }


    }
}
