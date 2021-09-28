using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsogenReportPreview.Models
{
    public class IsogenExcelColumn : IsogenExcelRow
    {
        public  string name { get; set; }

        public IsogenExcelColumn(string colname) : base()
        { 
            name = colname;
        }
        
        

        

       
    }
}
