using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsogenReportPreview.Models
{
    public class IsogenExcelColumn
    {
        CellValue[] _Cells;

        string name { get; set; }
        int cellcount { get; }

        public IsogenExcelColumn(string column_name, int count)
        {
            name = column_name;
            cellcount = count;
            _Cells = new CellValue[count];
        }
    
    }
}
