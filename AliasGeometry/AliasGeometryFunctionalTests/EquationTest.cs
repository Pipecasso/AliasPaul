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
    public class EquationTest
    {
        [TestMethod]
        public void EquationSolve1()
        {
            Equation e1 = new Equation(4, 5, -2, -14);
            Equation e2 = new Equation(7, -1, 2, 42);
            Equation e3 = new Equation(3, 1, 4, 28);

            Equation3 e = new Equation3(e1, e2, e3);
            double dx = 0;
            double dy = 0;
            double dz = 0;
            bool solved = e.Solve(ref dx, ref dy, ref dz);
            Assert.IsTrue(solved);
            Assert.IsTrue(dx == 4);
            Assert.IsTrue(dy == -4);
            Assert.IsTrue(dz == 5);

        }
    
    }
}
