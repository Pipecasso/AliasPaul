using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AutoSrpintReview
{
    public class AutoSprintReview
    {
        private List<BacklogItem> _backlogItems;
        private Dictionary<string, string> _columnIndexMap;
        private string[] _sharedStrings;
        private string _powertemplate;
        private string _teamName;
        private string _screenshotsPath;

        private Action<BacklogItem, string> ActionID = (x, y) => x.ID = y;
        private Action<BacklogItem, string> ActionTitle = (x, y) => x.Title = y;
        private Action<BacklogItem, string> ActionState = (x, y) => x.State = StringToState(y);
        private Action<BacklogItem, string> ActionPoints = (x, y) => x.Points = Convert.ToInt32(y);
        private Action<BacklogItem, string> ActionTags = (x, y) =>
        {
            List<string> tagList = new List<string>();
            string[] tags = y.Split(';');
            foreach (string tag in tags)
            {
                x.Tags.Add(tag);
            }
        };
        private Action<BacklogItem, string> ActionWorkItemType = (x, y) => x.WorkItemType = StringToWIT(y);

        private static BacklogItem.state StringToState(string sstate)
        {
            BacklogItem.state statetogo = BacklogItem.state.unset;
            switch (sstate)
            {
                case "Approved": statetogo = BacklogItem.state.approved; break;
                case "Committed": statetogo = BacklogItem.state.commited; break;
                case "Done": statetogo = BacklogItem.state.done; break;
                case "New": statetogo = BacklogItem.state.nnew; break;
            }
            return statetogo;
        }

        private static BacklogItem.workitemtype StringToWIT(string wit)
        {
            BacklogItem.workitemtype statetogo = BacklogItem.workitemtype.unset;
            switch (wit)
            {
                case "Product Backlog Item": statetogo = BacklogItem.workitemtype.productbi;break;
                case "Bug": statetogo = BacklogItem.workitemtype.bug;break;
            }
            return statetogo;

        }

        private static string CellIndex(Cell c, uint rowindex)
        {
            string cellref = c.CellReference;
            int i = cellref.IndexOf(rowindex.ToString());
            return cellref.Substring(0, i);
        }

         

        public AutoSprintReview(string xlsprint,string powertemplate,string teamname,string screenshotpath)
        {
            _backlogItems = new List<BacklogItem>();
            _powertemplate = powertemplate;
            _teamName = teamname;
            _screenshotsPath = screenshotpath;

            using (SpreadsheetDocument sr_doc = SpreadsheetDocument.Open(xlsprint, false))
            {
                Workbook workbook = sr_doc.WorkbookPart.Workbook;
                Worksheet worksheet = null;
                SharedStringTablePart sharedStringTablePart = workbook.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                _sharedStrings = sharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().Select(x => x.InnerText).ToArray();

                Sheet sheet = workbook.Descendants<Sheet>().FirstOrDefault();
                if (sheet != null)
                {
                    string relationshipId = sheet.Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)sr_doc.WorkbookPart.GetPartById(relationshipId);
                    worksheet = worksheetPart.Worksheet;
                }

                SheetData sheetData = worksheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Elements<Row>();

                _columnIndexMap = new Dictionary<string, string>();
                foreach (Row row in sheetData.Elements<Row>())
                {
                    uint rowindex = row.RowIndex;
                    switch (rowindex)
                    {
                        case 1: continue;
                        case 2:
                            {
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    int sharedindex = Convert.ToInt32(cell.CellValue.Text);
                                    _columnIndexMap.Add(_sharedStrings[sharedindex], CellIndex(cell, rowindex));
                                }
                            }
                            break;
                        default:
                            AddBacklogItem(row);
                            break;
                    }
                }
            }
        }


        private void AddBacklogItem(Row row)
        {
            BacklogItem backlogItem = new BacklogItem();
            ActionID(backlogItem, GetActionString("ID", row));
            ActionTitle(backlogItem, GetActionString("Title", row));
            ActionState(backlogItem, GetActionString("State", row));
            ActionPoints(backlogItem, GetActionString("Effort", row));
            ActionTags(backlogItem, GetActionString("Tags", row));
            ActionWorkItemType(backlogItem, GetActionString("Work Item Type", row));

            _backlogItems.Add(backlogItem);
           
        }

        private string GetActionString(string colname,Row row)
        {
            string celref = _columnIndexMap[colname] + row.RowIndex.ToString();
            Cell cell = row.Descendants<Cell>().Where(x => x.CellReference == celref).FirstOrDefault();
            string actionstring = string.Empty;
            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                int index = Convert.ToInt32(cell.CellValue.Text);
                actionstring = _sharedStrings[index];
            }
            else
            {
                actionstring = cell.CellValue.Text;
            }
            return actionstring;
        }

        public void MakeIt(string path)
        {
            const string baseURI = "https://dev.azure.com/hexagonPPMCOL/PPM/_boards/board/t/";
            PowerpointBacklogItems powerPointBacklogItems = new PowerpointBacklogItems(_backlogItems);
            foreach(PowerPointBacklogItem backlogItem in powerPointBacklogItems)
            {
                string link = $"{baseURI}{_teamName}/Backlog%20items/?workitem={backlogItem.ID}";
                string potential_imageloc = $@"{_screenshotsPath}\\{backlogItem.ID}";
                backlogItem.IDLink = link;
                if (Directory.Exists(potential_imageloc) && Directory.GetFiles(potential_imageloc).Any())
                {
                    backlogItem.ImagePath = potential_imageloc;
                }
            }
            
            using (PowerPoint powerPoint = new PowerPoint(powerPointBacklogItems, _powertemplate, @"C:\SR", "2"))
            {
                powerPoint.TeamName = _teamName;
                powerPoint.TeamDescription = "Isogen Futures";
                powerPoint.Date = DateTime.Now;
                powerPoint.LogoPath = @"C:\SR\mammap.jpg";
                powerPoint.AddBulletText(PowerPoint.BulletCat.SprintGoal, "Discover the meaning of life");
                powerPoint.AddBulletText(PowerPoint.BulletCat.SprintGoal, "Invent a new type of cheese");
                powerPoint.AddBulletText(PowerPoint.BulletCat.SprintGoal, "World Peace");
                powerPoint.AddBulletText(PowerPoint.BulletCat.Demo, "Anti Gravitiy Machine");
                powerPoint.AddBulletText(PowerPoint.BulletCat.Demo, "Runcornshire Cheese");
                powerPoint.MakeIt();
                powerPoint.SaveIt(path);
            }
        }
    }
}
