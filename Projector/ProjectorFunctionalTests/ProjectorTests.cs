﻿using System;
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

            Camera camera = new Camera(point, distance, vNormal);
            Assert.IsTrue(Math.Abs(Vector3d.Dot(camera.V1, camera.V2)) < double.Epsilon);
            double xt = camera.N.X - -23.4604321;
            Assert.IsTrue(Math.Abs(camera.N.X - -23.4604321) < 1e-6);
            Assert.IsTrue(Math.Abs(camera.N.Y - -5.96283) < 1e-6);
            Assert.IsTrue(camera.N.Z == 122);
        }

        [TestMethod]
        public void ProjectionTest()
        {
            double distance = 50;
            Vector3d vNormal = new Vector3d(6, -7, -2);
            vNormal.Normalise();
            Point3d point = new Point3d(-56, 32, 122);
            Camera camera = new Camera(point, distance, vNormal);
            Point2d projected_point = camera.ProjectPoint(new Point3d(400, -100, 23));
            Assert.IsTrue(projected_point.X == -32);
            Assert.IsTrue(projected_point.Y == -2);
        }




    }
}
