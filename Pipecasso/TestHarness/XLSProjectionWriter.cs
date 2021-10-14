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

        public XLSProjectionWriter(Dictionary<dynamic,Shapes3d> Shapes3d,Dictionary<dynamic,Shapes2d> Shapes2d)
        {
            _Shapes3d = Shapes3d;
            _Shapes2d = Shapes2d;
        }

        public void Go(string xlspath)
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(xlspath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                // Add a WorksheetPart to the WorkbookPart.
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Add Sheets to the Workbook.
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                    AppendChild<Sheets>(new Sheets());

                // Append a new worksheet and associate it with the workbook.
                Sheet line_sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                    GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Lines"
                };
                sheets.Append(line_sheet);

                Sheet cone_sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                   GetIdOfPart(worksheetPart),
                    SheetId = 2,
                    Name = "Cones"
                };
                sheets.Append(cone_sheet);
                
                uint linecount = 1;

                foreach (KeyValuePair<dynamic,Shapes3d> kvp in _Shapes3d)
                {
                    Shapes2d shapes2d = _Shapes2d[kvp.Key];
                    Shapes3d shapes3d = kvp.Value;

                    int tick2d = 0;
                    foreach (Line3d line3d in shapes3d.Lines)
                    {
                        Line2d line2d = shapes2d.Lines[tick2d];
                        //do it
                        tick2d++;
                        linecount++;
                    }
                  

                }
            }
    }
}
