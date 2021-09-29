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
            IsogenExcelReport excelReport = new IsogenExcelReport();
            excelReport.ParseSpreadsheet(path);

            Assert.AreEqual(8, excelReport.ColumnCount);
            Assert.AreEqual(7, excelReport.RowCount);
            Assert.AreEqual("PIPELINE-REFERENCE", excelReport[0].name);
            Assert.AreEqual("SPOOL-ID", excelReport[1].name);
            Assert.AreEqual("CENTRELINE-LENGTH", excelReport[4].name);
            Assert.AreEqual("8\"", excelReport[2, 3]);
           
        }
    
    }
}
