using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace IsogenReportPreview.Models
{
    public class IsogenExcelReport
    { 
        public IsogenExcelReport(string path)
        { 
        
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(path, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                string xml = sheetData.OuterXml;

                Dictionary<string, IsogenExcelColumn> columns = new Dictionary<string, IsogenExcelColumn>();
                foreach (Column c in sheetData.Elements<Column>())
                {
                    foreach (Cell cell in c.Elements<Cell>())
                    {
                        CellValue cv = cell.CellValue;
                        
                }
            }
        }

        public int ColumnCount
        {
            get 
            {
                return 0;
            }
        }
    
    }
}
