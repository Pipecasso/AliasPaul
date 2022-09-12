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
            ComplexNumber rsback = rs + _s;
            Assert.IsTrue(rsback.Equals(_r));
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
            ComplexNumber rback = rs * _s;
            Assert.IsTrue(_r == rback);
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
                new ComplexNumber(12,-5),new ComplexNumber(8.463486513975137,9.867593223667106),
                new ComplexNumber(-6.769277671037003,11.098507999384415),new ComplexNumber(-12.647130193961735,-3.00833805563494),
                new ComplexNumber(-1.047078648976399, -12.95776316741658)
            };
            foreach (ComplexNumber t in targets)
            {
                Assert.IsTrue(GapContains(roots,t,1e-9));
            }
        }

        [TestMethod]
        public void Sin()
        {
            ComplexNumber sin = _r.Sin();
            ComplexNumber sin_target = new ComplexNumber(48.75494167, -55.94196773);
            Assert.IsTrue(GapTestComplex(sin_target, sin, 1e-8));
        }

        [TestMethod]
        public void Cos()
        {
            ComplexNumber cos = _r.Cos();
            ComplexNumber cos_target = new ComplexNumber(55.94704749, 48.75051493);
            Assert.IsTrue(GapTestComplex(cos_target, cos, 1e-8));
        }

        [TestMethod]
        public void Tan()
        {
            ComplexNumber tan = _r.Tan();
            ComplexNumber tan_target = new ComplexNumber(8.994589181e-5, -0.9999876);
            Assert.IsTrue(GapTestComplex(tan_target, tan,1e-7));
        }

        [TestMethod]
        public void Sinh()
        {
            ComplexNumber shine = _s.Sinh();
            ComplexNumber shine_target = new ComplexNumber(199.6945123, 28.4661122);
            Assert.IsTrue(GapTestComplex(shine_target, shine,1e-7));
        }


        [TestMethod]
        public void Cosh()
        {
            ComplexNumber cosh = _s.Cosh();
            ComplexNumber cosh_target = new ComplexNumber(-199.6969662, -28.4657624);
            Assert.IsTrue(GapTestComplex(cosh_target, cosh, 1e-7));
        }

        [TestMethod]
        public void Tanh()
        {
            ComplexNumber tanh = _s.Tanh();
            ComplexNumber tanh_target = new ComplexNumber(0.002649, 0.9958218);
            Assert.IsTrue(!GapTestComplex(tanh_target, tanh, 1e-7));

        }

        [TestMethod]
        public void Log()
        {
            ComplexNumber rlog = _r.log();
            ComplexNumber slog = _s.log();
            ComplexNumber rslog = (_r * _s).log();
            ComplexNumber rplusslog = rlog + slog;
            ComplexNumber rcubedlog = (_r ^ 3).log();
            ComplexNumber threelogr = rlog * 3;


            Assert.IsTrue(GapTestComplex(new ComplexNumber(2.15203255, -0.62024949), rlog,1e-8));
            Assert.IsTrue(GapTestComplex(new ComplexNumber(1.90333124, 2.67794504), slog, 1e-8));
            Assert.IsTrue(GapTestComplex(rslog, rplusslog));
       
            Assert.IsTrue(GapTestComplex(rcubedlog, threelogr));
        }

        [TestMethod]
        public void Power()
        {
            ComplexNumber p = new ComplexNumber(1, 1);
            ComplexNumber ppp = p ^ p;
            double tp = Math.Sqrt(2) * Math.Exp(Math.PI/4);
            double qp = Math.PI / 4 + Math.Log(2) / 2;
            ComplexNumber target = new ComplexNumber(tp * Math.Cos(qp), tp * Math.Sin(qp));
            Assert.IsTrue(GapTestComplex(target, ppp, 1e-8));
        }

        [TestMethod]
        public void ii()
        {
            ComplexNumber i = new ComplexNumber(1,false);
            ComplexNumber ii = i ^ i;
            ComplexNumber target = new ComplexNumber(0.20787957635);
            Assert.IsTrue(GapTestComplex(target, ii));
        }
    }
}
