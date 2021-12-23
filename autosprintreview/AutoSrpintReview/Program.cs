using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AutoSrpintReview
{
    class Program
    {
        static string CellIndex(Cell c,uint rowindex)
        {
            string cellref = c.CellReference;
            int i = cellref.IndexOf(rowindex.ToString());
            return cellref.Substring(0, i);
        }
        
        static void Main(string[] args)
        {
            string document = args[0];
            using (SpreadsheetDocument sr_doc = SpreadsheetDocument.Open(document,false))
            {
                Workbook workbook = sr_doc.WorkbookPart.Workbook;
                SheetData sheetData = workbook.Descendants<SheetData>().First();
                BacklogItems backlogItems = new BacklogItems();
                Dictionary<string, string> columnIndexMap = new Dictionary<string, string>();
                foreach(Row row in  sheetData.Elements<Row>())
                {
                    uint rowindex = row.RowIndex;
                    switch (rowindex)
                    {
                        case 1:continue;
                        case 2:
                            {
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    columnIndexMap.Add(cell.CellValue.Text, CellIndex(cell, rowindex));
                                }
                            }
                            break;
                        default:
                            BacklogItem backlogItem = new BacklogItem(row.Descendants<Cell>());
                            backlogItems.Add(backlogItem);
                            break;
                    }
                }
            }
            
        }
    }
}
