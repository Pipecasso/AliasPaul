using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AliasGeometry;
using Projector;

namespace ProjectorFunctionalTests
{
    [TestClass]
    public class ProjectorTests
    {

        [TestMethod]
        public void ConstructorTest1()
        {
            double distance = 50;
            Vector3d vNormal = new Vector3d(6 / Math.Sqrt(85), -7 / Math.Sqrt(85), 0);
            Point3d point = new Point3d(-56,32,122);

            Projector.Projector projector = new Projector.Projector(point, distance, vNormal);
            Assert.IsTrue(Math.Abs(Vector3d.Dot(projector.V1, projector.V2)) < double.Epsilon);

        }




    }
}
