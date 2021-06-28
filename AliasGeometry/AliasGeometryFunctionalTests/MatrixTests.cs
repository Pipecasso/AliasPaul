using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using AliasGeometry;

namespace AliasGeometryFunctionalTests
{
    [TestClass]
    public class MatrixTests
    {
        private Matrix33 _m;
        public MatrixTests()
        {
            _m = new Matrix33();
            _m[0, 0] = 4;
            _m[0, 1] = 7;
            _m[0, 2] = 3;
            _m[1, 0] = 5;
            _m[1, 1] = -1;
            _m[1, 2] = 1;
            _m[2, 0] = -2;
            _m[2, 1] = 2;
            _m[2, 2] = 4;

         
        }

        [TestMethod]
        public void RowConstructor()
        {
            Vector3d c1 = new Vector3d(4, 5, -2);
            Vector3d c2 = new Vector3d(7, -1, 2);
            Vector3d c3 = new Vector3d(3, 1, 4);

            Matrix33 n = new Matrix33(c1, c2, c3,true);
            Assert.IsTrue(_m == n);
        }

        [TestMethod]
        public void ColumnConstructor()
        {
           Vector3d c1 = new Vector3d(4, 7, 3);
           Vector3d c2 = new Vector3d(5, -1, 1);
           Vector3d c3 = new Vector3d(-2, 2, 4);

            Matrix33 n = new Matrix33(c1, c2, c3, false);
            Assert.IsTrue(_m == n);
        }

        [TestMethod]
        public void Transpose()
        {
            Matrix33 t = Matrix33.Transpose(_m);
            Vector3d ctarget = new Vector3d(4, 7, 3);
            Vector3d rtarget = new Vector3d(4, 5, -2);

            Assert.IsTrue(_m.row(0) == rtarget);
            Assert.IsTrue(_m.column(0) == ctarget);

            for (int i=0;i<3;i++)
            {
                Assert.IsTrue(_m.row(i) == t.column(i));
                Assert.IsTrue(_m.column(i) == t.row(i));
            }

            Assert.IsTrue(_m.Determinant == t.Determinant);

        }
        [TestMethod]
        public void Determinant()
        {
            Assert.IsTrue(_m.Determinant == -154);
        }
    
    }
}
