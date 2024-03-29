﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AliasGeometry;
using Projector;
using AliasCubePoints;

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
        public void ConstructorTest2()
        {
            List<Point3d> pointList = ALotOfPoints.GetSomePoints();
            CubeView cube = new CubeView(pointList);

            Vector3d vNormal = new Vector3d(cube.BackBottomLeft, cube.FrontTopRight);
            vNormal.Normalise();
            const int width = 400;
            const int height = 250;

            Projector.Camera camera = new Camera(vNormal, cube, 7,4,width,height);

            Point2d topleft = new Point2d(-width / 2, height / 2);
            Point2d bottomright = new Point2d(width / 2, -height / 2);
            Rectangle2d rtangle = new Rectangle2d(topleft, bottomright);
            List<double> distances = new List<double>();




            
            List<Point2d> projectedPoints = new List<Point2d>();
            foreach (Point3d p in pointList)
            {
                Point2d projpoint = camera.ProjectPoint(p);
                projectedPoints.Add(projpoint);
            }

            foreach (Point2d p in projectedPoints)
            {
                Assert.IsTrue(rtangle.IsPointInside(p));
                distances.Add(rtangle.PointDistance(p));
            }

            double nearesttoborder = distances.Min();
            Assert.IsTrue(Math.Abs(nearesttoborder) < 1);
            
            
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

        [TestMethod]
        public void DistanceCalculationVertial()
        {
            const double camdist = 6000;
            Vector3d vNormal = new Vector3d(0, 1, 1);
            vNormal.Normalise();
            Point3d CubeCenter = new Point3d(-6, -146, -18);
            Point3d CameraPoint = CubeCenter - vNormal * camdist;
        

            Projector.Camera camera = new Camera(CameraPoint, 200, vNormal);
            Point3d ptest = new Point3d(653, 779, 954);
            
        
            int height = 100;
          

            double dDist = camera.CalculateDistanceRequiredForPoint(ptest, true, height);
            Projector.Camera testcam = new Camera(CameraPoint, dDist, vNormal);
            Point2d phope = testcam.ProjectPoint(ptest);
            Assert.IsTrue(phope.Y == height);

         
        }

        [TestMethod]
        public void DistanceCalculationHorizontal()
        {
            const double camdist = 5000;
            Vector3d vNormal = new Vector3d(0, 1, 1);
            vNormal.Normalise();
            Point3d CubeCenter = new Point3d(-6, -146, -18);
            Point3d CameraPoint = CubeCenter - vNormal * camdist;
            double disttest = Point3d.Distance(CubeCenter, CameraPoint);
           
             Projector.Camera camera = new Camera(CameraPoint, 200, vNormal);


            int width = 250;
            Point3d ptest = new Point3d(-860, 858, 351);
            double dDist = camera.CalculateDistanceRequiredForPoint(ptest, false, width);
            Projector.Camera testcam = new Camera(CameraPoint, dDist, vNormal);
            Point2d phope = testcam.ProjectPoint(ptest);
            Assert.AreEqual(-width, phope.X);

        }




    }
}
