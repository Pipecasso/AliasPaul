using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IsogenReportPreview.Models;

namespace IsogenReportingPreviewTests
{
    [TestClass]
    public class ExcelReportTest
    {
        [DeploymentItem("SpoolInformationFileImperial.xlsx")]
        [TestMethod]
        public void ParseSpreadsheet()
        {
            string path = "SpoolInformationFileImperial.xlsx";
            IsogenExcelReport excelReport = new IsogenExcelReport(path);

            Assert.AreEqual(8, excelReport.ColumnCount);
           
        }
    
    }
}
