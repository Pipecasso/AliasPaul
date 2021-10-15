using AliasGeometry;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Projector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHarness
{
    public class XLSProjectionWriter
    {
        private Dictionary<dynamic, Shapes3d> _Shapes3d;
        private Dictionary<dynamic, Shapes2d> _Shapes2d;
        private SpreadsheetDocument _spreadsheetDocument;
        private WorkbookPart _workbookPart;
        private Sheets _sheets;

        public XLSProjectionWriter(Dictionary<dynamic, Shapes3d> Shapes3d, Dictionary<dynamic, Shapes2d> Shapes2d, string xlspath)
        {
            _Shapes3d = Shapes3d;
            _Shapes2d = Shapes2d;

            _spreadsheetDocument = SpreadsheetDocument.Create(xlspath, SpreadsheetDocumentType.Workbook);
           
            _workbookPart = _spreadsheetDocument.AddWorkbookPart();
            _workbookPart.Workbook = new Workbook();

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = _workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add Sheets to the Workbook.
            _sheets  = _spreadsheetDocument.WorkbookPart.Workbook.
                AppendChild<Sheets>(new Sheets());
            
        }
 
        public void Go()
        {
            WorksheetPart worksheetPart = _workbookPart.WorksheetParts.First();
            // Append a new worksheet and associate it with the workbook.
            Sheet line_sheet = new Sheet()
            {
                Id = _spreadsheetDocument.WorkbookPart.
                GetIdOfPart(_workbookPart.WorksheetParts.First()),
                SheetId = 1,
                Name = "Lines"
            };
            _sheets.Append(line_sheet);

            uint linecount = 1;

            SharedStringTablePart shareStringPart;
            if (_workbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
            {
                shareStringPart = _workbookPart.GetPartsOfType<SharedStringTablePart>().First();
            }
            else
            {
                shareStringPart = _workbookPart.AddNewPart<SharedStringTablePart>();
            }

            foreach (KeyValuePair<dynamic, Shapes3d> kvp in _Shapes3d)
            {
                Shapes2d shapes2d = _Shapes2d[kvp.Key];
                Shapes3d shapes3d = kvp.Value;

                int tick2d = 0;
                foreach (Line3d line3d in shapes3d.Lines)
                {
                    Line2d line2d = shapes2d.Lines[tick2d];
                    string uci = kvp.Key.ExternalUCI;
                    InsertIntoCell("A", linecount, shareStringPart, worksheetPart, line3d.P.X);
                    InsertIntoCell("B", linecount, shareStringPart, worksheetPart, line3d.P.Y);
                    InsertIntoCell("C", linecount, shareStringPart, worksheetPart, line3d.P.Z);

                    InsertIntoCell("E", linecount, shareStringPart, worksheetPart, line3d.Q.X);
                    InsertIntoCell("F", linecount, shareStringPart, worksheetPart, line3d.Q.Y);
                    InsertIntoCell("G", linecount, shareStringPart, worksheetPart, line3d.Q.Z);
                    InsertIntoCell("I", linecount, shareStringPart, worksheetPart, uci);

                    InsertIntoCell("K", linecount, shareStringPart, worksheetPart, line2d.start.X);
                    InsertIntoCell("L", linecount, shareStringPart, worksheetPart, line2d.start.Y);
                    InsertIntoCell("N", linecount, shareStringPart, worksheetPart, line2d.end.X);
                    InsertIntoCell("O", linecount, shareStringPart, worksheetPart, line2d.end.Y);

                    linecount++;

                }
                tick2d++;
            }
            _spreadsheetDocument.Save();
            _spreadsheetDocument.Dispose();
            _spreadsheetDocument = null;

        }

        private static void InsertIntoCell(string col, uint row, SharedStringTablePart sharedStringTablePart, WorksheetPart worksheetPart, int val)
        {
            string text = val.ToString();
            Cell cell = InsertCellInWorksheet(col, row, worksheetPart);
            cell.CellValue = new CellValue(text);
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
        }

        private static void InsertIntoCell(string col, uint row, SharedStringTablePart sharedStringTablePart, WorksheetPart worksheetPart, double val)
        {
            string text = val.ToString();
            Cell cell = InsertCellInWorksheet(col, row, worksheetPart);
            cell.CellValue = new CellValue(text);
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
        }

        private static void InsertIntoCell(string col, uint row, SharedStringTablePart sharedStringTablePart, WorksheetPart worksheetPart, string val, CellValues cellValues = CellValues.SharedString)
        {
            int index = InsertSharedStringItem(val, sharedStringTablePart);
            Cell cell = InsertCellInWorksheet(col, row, worksheetPart);
            cell.CellValue = new CellValue(index.ToString());
            cell.DataType = new EnumValue<CellValues>(cellValues);
        }

        private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }

        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (cell.CellReference.Value.Length == cellReference.Length)
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }
    }
}
