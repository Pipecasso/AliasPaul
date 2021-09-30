using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using IsogenReportPreview.Models;

namespace IsogenReportingPreviewTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class IsogenExcelColumnTests
    {        
        public IsogenExcelColumn MakeColumn()
        {
            IsogenExcelColumn myColumn = new IsogenExcelColumn("TestColumn");
            for (int i=0;i<7;i++)
            {
                myColumn.AddString(i.ToString());
            }
            myColumn.AddString("Row8");
            return myColumn;
        }

        [TestMethod]
        public void ValueTest()
        {
            IsogenExcelColumn myColumn = MakeColumn();
            Assert.AreEqual("TestColumn", myColumn.name);
            Assert.AreEqual(8, myColumn.CellCount);
            for(int i=0;i<7;i++)
            {
                string cellvalue = myColumn[i];
                string expectedvalue = i.ToString();
                Assert.AreEqual(expectedvalue, cellvalue);
            }
            Assert.AreEqual("Row8", myColumn[7]);
        }
    
    
    }
}
