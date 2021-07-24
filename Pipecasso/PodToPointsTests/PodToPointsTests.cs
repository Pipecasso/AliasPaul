using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AliasPOD;
using ManagedPodLoader;
using System.IO;
using PodToPoints;
using AliasGeometry;


namespace PodToPointsTests
{

    [DeploymentItem(@"..\..\..\..\bin\Debug\Core Components\", "Core Components")]
    [DeploymentItem(@"..\..\..\..\bin\Debug\Application Components\", "Application Components")]
    [DeploymentItem(@"..\..\..\..\bin\Debug\Application Components\", "Paul Components")]
    [DeploymentItem(@"..\..\..\..\bin\Debug\Shared.Interop.POD.dll")]
    [DeploymentItem(@"..\..\..\..\bin\Debug\Shared.Interop.MaterialDataWrapper.dll")]
    [DeploymentItem(@"..\..\..\..\bin\Debug\Application Components\BWFC.dll")]
    [DeploymentItem(@"..\..\..\TestData\", "TestData")]
    [DeploymentItem(@"PodToPointsTests.SxS.manifest")]
    





    [TestClass]
    public class PodToPointsTests
    {

     

        [TestMethod]
        public void PodToCube()
        {
            double tolerance = 1e-3;
            string podFile = "Pipecasso1.POD";
            string podPath = Path.Combine("TestData", podFile);
            POD pod = Handshaker.LoadPOD(podPath);

            PODTransformer transformer = new PODTransformer(pod);
            CubeView cubeView = transformer.GetCube();
            Assert.IsTrue(InRange(5347.69, cubeView.FrontTopLeft.Z,tolerance));
            Assert.IsTrue(InRange(-5000, cubeView.BackBottomLeft.Z, tolerance));
            Assert.IsTrue(InRange(12356.91, cubeView.BackBottomRight.X, tolerance));
            Assert.IsTrue(InRange(-6944.2, cubeView.FrontBottomRight.X, tolerance));
            Assert.IsTrue(InRange(9095.9312, cubeView.BackTopLeft.Y, tolerance));
            Assert.IsTrue(InRange(-2026.71, cubeView.FrontTopRight.Y, tolerance));
            Point3d cubeCenter = cubeView.Center;
            Assert.IsTrue(InRange(2706.355, cubeCenter.X, tolerance));
            Assert.IsTrue(InRange(3534.6106, cubeCenter.Y, tolerance));
            Assert.IsTrue(InRange(173.845, cubeCenter.Z, tolerance));


        }

        private bool InRange(double expected,double actual,double range)
        {
            return Math.Abs(expected - actual) <= range;
        }
    
    
    }
}
