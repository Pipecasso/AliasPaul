using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mocka;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
using AliasGeometry;

namespace AliasGeometryUnitTests
{
    [TestClass]
    public class BoundedPlaneTest
    {
        private MockCreationHelper _mockCreationHelper;
        public BoundedPlaneTest()
        {
            _mockCreationHelper = new MockCreationHelper();
        }   
    }
}
