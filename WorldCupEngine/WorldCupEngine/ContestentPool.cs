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
        public ContestentPool(string openxmlpath)
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(openxmlpath,true))
            {

            }

        }
    
    
    }
}
