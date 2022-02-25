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
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(openxmlpath,false))
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

 

        public void Export(string openxmlpath,string sheetname)
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(openxmlpath, true))
            {
                Workbook workbook = spreadsheetDocument.WorkbookPart.Workbook;
                SharedStringTablePart sharedStringTablePart = workbook.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                IEnumerable<string> sharedstrings = sharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().Select(x => x.InnerText);
                Dictionary<string, int> sharedStringDic = new Dictionary<string, int>();
                int tick = 0;
                foreach (string s in sharedstrings)
                {
                    sharedStringDic.Add(s, tick);
                    tick++;
                }

                Worksheet contestentwsheet = null;
                IEnumerable<Sheet> sheets = workbook.Descendants<Sheet>();
                Sheet contestentsheet = workbook.Descendants<Sheet>().Where(x => x.Name == sheetname).FirstOrDefault();
                if (contestentsheet != null)
                {
                    string relationshipId = contestentsheet.Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(relationshipId);
                    contestentwsheet = worksheetPart.Worksheet;
                    SheetData sheetData = contestentwsheet.GetFirstChild<SheetData>();
                    IEnumerable<Cell> Cells = sheetData.Elements<Row>().Select(x => x.Descendants<Cell>()).First();
                    IEnumerable<String> CellValues = Cells.Select(x => x.CellValue.Text);

                    Dictionary<string, int> cv_dic = new Dictionary<string, int>();
                    tick = 0;
                    foreach (string cv in CellValues)
                    {
                        cv_dic.Add(cv, tick);
                        tick++;
                    }

                    foreach (Contestent contestent in this)
                    {
                        int ssindex = sharedStringDic[contestent.Name];
                        int rowindex = cv_dic[ssindex.ToString()];
                        Row row = sheetData.Elements<Row>().ToArray()[rowindex];

                        Cell[] cells = row.Descendants<Cell>().ToArray();
                        Cell picked = cells[1];
                        picked.CellValue = new CellValue(contestent.Tornaments.ToString());
                        Cell champion = cells[7];
                        champion.CellValue = new CellValue(contestent.TournementWins.ToString());
                        Cell wins = cells[8];
                        wins.CellValue = new CellValue(contestent.Wins.ToString());
                        Cell losses = cells[9];
                        losses.CellValue = new CellValue(contestent.Losses.ToString());
                    }
                }
            }
        }
    }
}
