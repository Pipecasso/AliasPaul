using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliasGeometry
{
    public class Line3d
    {
        private Point3d _p;
        private Point3d _q;

        public Line3d(Point3d p,Point3d q)
        {
            _p = p;
            _q = q;
        }

        public Point3d P
        {
            get
            {
                return _p;
            }
        }

        public Point3d Q
        {
            get
            {
                return _q;
            }
        }

        public double Length()
        {
            return Point3d.Distance(_p, _q);
        }

		public Vector3d Vector(bool normalise)
        {
			Vector3d togo = new Vector3d(_p, _q);
			if (normalise) togo.Normalise();
			return togo;
        }

        static public bool Intersection(Line3d l1,Line3d l2,ref Point3d I,double tolerance = double.Epsilon)
        {
			Point3d P = l1.P;
			Point3d Q = l2.P;
			


			Vector3d LineDir = l1.Vector(true);
			Vector3d CheckLineDir = l2.Vector(true);
	
			double ni, nj, nk;
			double ri, rj, rk;
			ni = CheckLineDir.X;
			nj = CheckLineDir.Y;
			nk = CheckLineDir.Z;
			ri = LineDir.X;
			rj = LineDir.Y;
			rk = LineDir.Z;

			Matrix22 m12 = new Matrix22(ni,-ri,nj,-rj);
			Matrix22 m13 = new Matrix22(ni,-ri,nk,-rk);
			Matrix22 m23 = new Matrix22(nj,-rj,nk,-rk);
			Matrix22 m12_1;
			Matrix22 m13_1;
			Matrix22 m23_1;
			bool bInvert12 = m12.Determinant != 0;
			bool bInvert13 = m13.Determinant != 0;
			bool bInvert23 = m23.Determinant != 0;

			Vector2d C12 = new Vector2d(P.X- Q.X,P.Y - Q.Y);
			Vector2d C13 = new Vector2d(P.X- Q.X,P.Z - Q.Z);
			Vector2d C23 = new Vector2d(P.Y - Q.Y,P.Z - Q.Z);

			Vector2d solution12 = new Vector2d();
			Vector2d solution13 = new Vector2d();
			Vector2d solution23 = new Vector2d();
			int iInvertCount = 0;

			if (bInvert12)
			{
				m12_1 = m12.Inverse();
				solution12 = m12_1 * C12;
				iInvertCount++;
			}

			if (bInvert13)
			{
				m13_1 = m13.Inverse();
				solution13 = m13_1 * C13;
				iInvertCount++;
			}

			if (bInvert23)
			{
				m23_1 = m23.Inverse();
				solution23 = m23_1 * C23;
				iInvertCount++;
			}

			double theta, lambda;
			bool bReturn = false;
			Point3d checkPos = new Point3d();

			if (iInvertCount == 3)
			{
                Vector2d error1223 = solution12 - solution23;
                Vector2d error1213 = solution12 - solution13;
                if (error1223.Magnitude() <= tolerance && error1213.Magnitude()<=tolerance)
				{
					theta = solution12.Item1; //n and LineDir & Q
					lambda = solution12.Item2;//r and CheckLineDir + P
					I = P + (LineDir * lambda);
					checkPos = Q + (CheckLineDir * theta);
					bReturn = true;
				}
			}
			else if (iInvertCount == 2)
			{
				//find the imposter
				if (!bInvert12)
				{
                    Vector2d error2313 = solution23 - solution13;
                    if (error2313.Magnitude() <= tolerance)
					{
						theta = solution23.Item1; //n and LineDir & Q
						lambda = solution23.Item2;//r and CheckLineDir + P
						I = P + (LineDir * lambda);
						checkPos = Q + (CheckLineDir * theta);
						bReturn = true;
					}
				}
				else if (!bInvert13)
				{
                    Vector2d error1223 = solution12 - solution23;
                    if (error1223.Magnitude() <= tolerance)
					{
						theta = solution12.Item1; //n and LineDir & Q
						lambda = solution12.Item2;//r and CheckLineDir + P
						I = P + (LineDir * lambda);
						checkPos = Q + (CheckLineDir * theta);
						bReturn = true;
					}
				}
				else if (!bInvert23)
				{
                    Vector2d error1312 = solution13 - solution12;
                    if (error1312.Magnitude() <= tolerance)
					{
						theta = solution12.Item1; //n and LineDir & Q
						lambda = solution12.Item2;//r and CheckLineDir + P
						I = P + (LineDir * lambda);
						checkPos = Q + (CheckLineDir * theta);
						bReturn = true;
					}
				}
			}
			else if (iInvertCount == 1)
			{
				if (bInvert12)
				{
					theta = solution12.Item1; //n and LineDir & Q
					lambda = solution12.Item2;//r and CheckLineDir + P
					I = P + (LineDir * lambda);
					checkPos = Q + (CheckLineDir * theta);
					bReturn = true;
				}
				else if (bInvert13)
				{
					theta = solution13.Item1; //n and LineDir & Q
					lambda = solution13.Item2;//r and CheckLineDir + P
					I = P + (LineDir * lambda);
					checkPos = Q + (CheckLineDir * theta);
					bReturn = true;
				}
				else
				{
					theta = solution23.Item1; //n and LineDir & Q
					lambda = solution23.Item2;//r and CheckLineDir + P
					I = P + (LineDir * lambda);
					checkPos = Q + (CheckLineDir * theta);
					bReturn = true;
				}
			}

			if (bReturn == true)
			{
				Vector3d DiffPos = new Vector3d(checkPos, I);
				double dError = DiffPos.Magnitude();
				bReturn = dError <= tolerance;
			}
			return bReturn;
		}

    }
}
