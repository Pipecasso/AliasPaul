using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;


namespace WorldCupEngine
{
    public class ContestentPool : List<Contestent>
    {
        public ContestentPool(string openxmlpath,string sheetname)
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(openxmlpath,true))
            {
                Workbook workbook = spreadsheetDocument.WorkbookPart.Workbook;
                SharedStringTablePart sharedStringTablePart = workbook.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();

                Worksheet contestentwsheet = null;
                IEnumerable<Sheet> sheets = workbook.Descendants<Sheet>();
                Sheet contestentsheet = workbook.Descendants<Sheet>().Where(x => x.Name == sheetname).FirstOrDefault();
                if (contestentsheet != null)
                {
                    string relationshipId = contestentsheet.Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(relationshipId);
                    contestentwsheet = worksheetPart.Worksheet;
                    SheetData sheetData = contestentwsheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rows = sheetData.Elements<Row>();
                    IEnumerable<string> contestent_indexlist = rows.Select(x => x.Descendants<Cell>().First().CellValue.Text);
                    string[] sharedStrings = sharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().Select(x => x.InnerText).ToArray();
                   
                    foreach (string index in contestent_indexlist)
                    {
                        int iindex = Convert.ToInt32(index);
                        Contestent contestent = new Contestent()
                        {
                            Name = sharedStrings[iindex]
                        };
                        Add(contestent);
                    }
                }
              
            }

        }
    
    
    }
}
