using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;



namespace WorldCupEngine
{
    public class ContestentPool : List<Contestent>
    {
        private string _sheetname;
        public ContestentPool(string openxmlpath,string sheetname)
        {
            XLWorkbook wb = new XLWorkbook(openxmlpath);
            IXLWorksheet ws = wb.Worksheets.Where(x => x.Name == sheetname).FirstOrDefault();
            if (ws == null)
            {
                ws = wb.Worksheets.First();
                _sheetname = ws.Name;
            }
            else
            {
                _sheetname = sheetname;
            }    
            foreach (IXLRow row in ws.Rows())
            {
                Contestent contestent = new Contestent();
                contestent.Name = row.Cell("A").GetString();
          
                contestent.Tornaments = row.Cell("B").GetValue<int>();
                contestent.TournementWins = row.Cell("C").GetValue<int>();
                contestent.Wins = row.Cell("D").GetValue<int>();
                contestent.Losses = row.Cell("E").GetValue<int>();
                contestent.Points = row.Cell("F").GetValue<int>();
                
              
                Add(contestent);
            }
        }
        
        public string SheetName { get => _sheetname;}

     
    }
}
