using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFilter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoTests
{
    [TestClass]
    public class TMatrixTests
    {
        [TestMethod]
        public void SaveAndLoad()
        {
            Func<int, int, double> fk = (x, y) => x * y - (2*x - y) ^ 3 - 4*x + 3*y;
            TransformMatrix tm = new TransformMatrix(1000);
            tm.Set(fk, "xy - (2x-y)^3 - 4x + 3y");
            tm.Save("test.tmx");

            TransformMatrix tmload = new TransformMatrix(1000);
            tmload.Load("test.tmx");

            Assert.IsTrue(tmload == tm);

        }
    }
}
