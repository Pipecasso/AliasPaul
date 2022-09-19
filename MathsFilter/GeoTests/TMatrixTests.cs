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
    public class MatrixTests
    {
        [TestMethod]
        public void SaveAndLoad()
        {
            Func<double, double, double> fk = (x, y) => x * y - Math.Pow((2*x - y),3) - 4*x + 3*y;
            TransformMatrix tm = new TransformMatrix(1000);
            tm.Set(fk, "xy - (2x-y)^3 - 4x + 3y");
            tm.Save("test.tmx");

            TransformMatrix tmload = new TransformMatrix();
            tmload.Load("test.tmx");

            Assert.IsTrue(tmload == tm);

        }

        [TestMethod]
        public void ComplexSaveandLoad()
        {
            Func<ComplexNumber, ComplexNumber> func = (z) => (z ^ 2) - (z * 5.0);
            ZMatrix zm = new ZMatrix(1000);
            zm.Set(func, "z^2 -5z");
            zm.Save("test.zmx");

            ZMatrix zmload = new ZMatrix();
            zmload.Load("test.zmx");

            Assert.IsTrue(zmload == zm);
        }
    }
}
