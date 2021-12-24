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
            string powerpath = args[1];
            string templatepath = args[2];
            BacklogItems backlogItems = new BacklogItems();

            using (SpreadsheetDocument sr_doc = SpreadsheetDocument.Open(document,false))
            {
                Workbook workbook = sr_doc.WorkbookPart.Workbook;
                Worksheet worksheet = null;
                SharedStringTablePart sharedStringTablePart = workbook.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                string[] sharedStrings = sharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().Select(x => x.InnerText).ToArray();

                Sheet sheet = workbook.Descendants<Sheet>().FirstOrDefault();
                if (sheet != null)
                {
                    string relationshipId = sheet.Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)sr_doc.WorkbookPart.GetPartById(relationshipId);
                    worksheet = worksheetPart.Worksheet;
                }

                SheetData sheetData = worksheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Elements<Row>();
               
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
                                    int sharedindex = Convert.ToInt32(cell.CellValue.Text);
                                    columnIndexMap.Add(sharedStrings[sharedindex], CellIndex(cell, rowindex));
                                }
                            }
                            break;
                        default:
                            BacklogItem backlogItem = new BacklogItem(row.Descendants<Cell>(),columnIndexMap,rowindex,sharedStrings);
                            backlogItems.Add(backlogItem);
                            break;
                    }
                }
            }

            using (PowerPoint powerPoint = new PowerPoint(backlogItems, templatepath))
            {
                powerPoint.MakeIt();


            }

          

        }
    }
}
