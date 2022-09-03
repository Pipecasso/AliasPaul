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
    public class ComplexTests
    {
        private ComplexNumber _r;
        private ComplexNumber _s;

        public ComplexTests()
        {
            _r = new ComplexNumber(7,-5);
            _s = new ComplexNumber(-6, 3);
        }

        [TestMethod]
        public void PolarConstructor()
        {
            ComplexNumber polars = new ComplexNumber(3 * Math.Sqrt(5), 2.677945045, false);
            Assert.IsTrue(GapTestComplex(_s, polars,1e-08));
        }

        private bool GapTestDouble(double want, double got, double tolerance= 1e-10)
        {
            double Gap = Math.Abs(want - got);
            return Gap < tolerance;
        }
       
        private bool GapTestComplex(ComplexNumber want,ComplexNumber got,double tolerance = 1e-10)
        {
            return GapTestDouble(want.X,got.X,tolerance) && GapTestDouble(want.Y,got.Y,tolerance);    
        }

        private bool GapContains(List<ComplexNumber> complexNumbers,ComplexNumber want, double tolerance = 1e-10)
        {
            bool gotit = false;
            foreach (ComplexNumber complexNumber in complexNumbers)
            {
                if (GapTestComplex(want,complexNumber,tolerance))
                {
                    gotit = true;
                    break;
                }
            }
            return gotit;
        }


        [TestMethod]
        public void AddTest()
        {
            ComplexNumber rs = _r + _s;
            Assert.AreEqual(1, rs.X);
            Assert.AreEqual(-2, rs.Y);
        }

        [TestMethod]
        public void SubractTest()
        {
            ComplexNumber rs = _r - _s;
            Assert.AreEqual(13, rs.X);
            Assert.AreEqual(-8, rs.Y);
        }

        [TestMethod]
        public void ModTest()
        {
            Assert.AreEqual(Math.Sqrt(74), _r.Mod);
        }

        [TestMethod]
        public void ArgTest()
        {
            Assert.IsTrue(Math.Abs(_r.Arg + 0.620249486) < 1e-8);
        }

        [TestMethod]
        public void MultTest()
        {
            ComplexNumber rs = _r * _s;
            Assert.AreEqual(-27, rs.X);
            Assert.AreEqual(51, rs.Y);
        }

        [TestMethod]
        public void DivTest()
        {
            ComplexNumber rs = _r / _s;
            Assert.AreEqual(-19.0 / 15.0, rs.X);
            Assert.AreEqual(0.2,rs.Y);
        }

        [TestMethod] 
        public void Mult1()
        {
            ComplexNumber r6 = _r ^ 6;
            ComplexNumber target = new ComplexNumber(-338976, 222040);
            Assert.IsTrue(GapTestComplex(target,r6));
            Assert.IsTrue(GapTestComplex (target, _r * _r * _r * _r * _r * _r));
        }

        [TestMethod]
        public void Mult2()
        {
            ComplexNumber r3 = _r ^ -3;
            ComplexNumber target = new ComplexNumber(-91.0 / 202612.0, 305.0 / 202612.0);
            Assert.IsTrue(GapTestComplex(target, r3));
            ComplexNumber one = new ComplexNumber(1);
            Assert.IsTrue(GapTestComplex(target, one / (_r * _r * _r)));
        }

        [TestMethod]
        public void SqrRoot()
        {
            List<ComplexNumber> roots = _s.Root(2);
            Assert.AreEqual(2, roots.Count);
            foreach (ComplexNumber r in roots)
            {
                Assert.IsTrue(GapTestComplex(_s, r*r));
            }

            ComplexNumber targetone = new ComplexNumber(0.595064674005847, 2.520734410097518);
            ComplexNumber targettwo = targetone * -1.0;
            Assert.IsTrue(GapContains(roots, targetone));
            Assert.IsTrue(GapContains(roots, targettwo));
        }

        [TestMethod]
        public void Root5()
        {
            ComplexNumber c5 = new ComplexNumber(-145668, -341525);
            List<ComplexNumber> roots = c5.Root(5);
            Assert.AreEqual(5, roots.Count);
            foreach (ComplexNumber r in roots)
            {
                Assert.IsTrue(GapTestComplex(c5, r ^ 5,1e-9));
            }
            List<ComplexNumber> targets = new List<ComplexNumber>()
            {
                new ComplexNumber(12,-5),new ComplexNumber(8.463486513975137,-9.867593223667106),
                new ComplexNumber(-6.769277671037003,11.098507999384415),new ComplexNumber(-12.647130193961735,-3.00833805563494),
                new ComplexNumber(-1.047078648976399, -12.95776316741658)
            };
            foreach (ComplexNumber t in targets)
            {
                Assert.IsTrue(GapContains(roots,t,1e-7));
            }
        }

    }
}
