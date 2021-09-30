using ISOGENControlPanelTests;
using IsogenReportPreview.Models;
using IsogenReportPreview.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
using TestUtilities;

namespace IsogenReportingPreviewTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ColumnViewModelTests
    {
        private IsogenReportPreviewColumnViewModel _isogenReportPreviewColumnViewModel;
        private readonly MockCreationHelper _mockCreationHelper = new MockCreationHelper();
        private IsogenExcelColumn _mockIsogenExcelColumn;

        [TestInitialize]
        public void TestInitialise()
        {

            _isogenReportPreviewColumnViewModel = new IsogenReportPreviewColumnViewModel();
            _mockIsogenExcelColumn = _mockCreationHelper.Create<IsogenExcelColumn>();
          /*  _mockIsogenExcelColumn.Arrange(x => x[0]).Returns("Cell0");
            _mockIsogenExcelColumn.Arrange(x => x[1]).Returns("Cell1");
            _mockIsogenExcelColumn.Arrange(x => x[2]).Returns("Cell2");
            _mockIsogenExcelColumn.Arrange(x => x[3]).Returns("Cell3");*/
         
        }
    
        public void TestCleanup()
        {
            _mockCreationHelper.AssertAll();
        }

        [TestMethod]
        public void NoColumnNoName()
        {
            Assert.AreEqual(string.Empty, _isogenReportPreviewColumnViewModel.HeaderName);
        }

        [TestMethod]
        public void ColumnName()
        {
            _mockIsogenExcelColumn.Arrange(x => x.name).Returns("Header");
            _isogenReportPreviewColumnViewModel.IsogenExcelColumn = _mockIsogenExcelColumn;
            Assert.AreEqual("Header", _isogenReportPreviewColumnViewModel.HeaderName);
        }
    }
}
