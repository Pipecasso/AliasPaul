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

        IsogenExcelColumn[] _columns;
        
        public void ParseSpreadsheet(string path)
        {
            _columns = null;
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(path, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                SharedStringTablePart stringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

                int rowcount = sheetData.Elements<Row>().Count();
                int colcount = sheetData.Elements<Row>().First().Elements<Cell>().Count();
                _columns = new IsogenExcelColumn[colcount];

                bool skiponerow = true;

                foreach (Row r in sheetData.Elements<Row>())
                {
                    int tick = 0;
                    if (_columns[0] == null)
                    {
                        //nameline
                        foreach (Cell c in r.Elements<Cell>())
                        {
                            string value = stringTable.SharedStringTable.ElementAt(int.Parse(c.InnerText)).InnerText;
                            IsogenExcelColumn isogenExcelColumn = new IsogenExcelColumn(value);
                            _columns[tick] = isogenExcelColumn;
                            tick++;
                        }
                    }
                    else
                    {
                        if (skiponerow)
                        {
                            skiponerow = false;
                            continue;
                        }
                        else
                        {
                            foreach (Cell c in r.Elements<Cell>())
                            {
                                string nextcell = string.Empty;
                                IsogenExcelColumn isogenExcelColumn = _columns[tick];
                                if (c.DataType != null && c.DataType == CellValues.SharedString)
                                {
                                    nextcell = stringTable.SharedStringTable.ElementAt(int.Parse(c.InnerText)).InnerText;
                                }
                                else
                                {
                                    nextcell = c.CellValue.Text;
                                }
                                isogenExcelColumn.AddString(nextcell);
                                tick++;
                            }
                        }
                    }
                }
            }
        }

        public int ColumnCount
        {
            get
            {
                return _columns.Length;
            }
        }

        public int RownCount
        {
            get
            {
                int retout = 0;
                if (_columns.Length > 0)
                {
                    retout = _columns[0].CellCount;
                }
                return retout;
            }
        }

        public IsogenExcelColumn this[int i]
        { 
            get
            {
                return _columns[i];
            }
        }

        public string this[int i,int j]
        {
            get
            {
                return (this[i])[j];
            }

        }
    }
}
