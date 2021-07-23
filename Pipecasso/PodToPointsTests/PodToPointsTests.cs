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
            string podFile = "Pipecasso1.POD";
            string podPath = Path.Combine("TestData", podFile);
            POD pod = Handshaker.LoadPOD(podPath);

            PODTransformer transformer = new PODTransformer(pod);
            CubeView cubeView = transformer.GetCube();
            Assert.AreEqual(5000, cubeView.FrontTopLeft.Z);
            
        
        }
    
    
    }
}
