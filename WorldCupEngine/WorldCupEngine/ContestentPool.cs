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
        public ContestentPool(string openxmlpath,string sheetname,bool firstrun = true)
        {
            XLWorkbook wb = new XLWorkbook(openxmlpath);
            IXLWorksheet ws = wb.Worksheets.Where(x => x.Name == sheetname).FirstOrDefault();
            if (ws == null) ws = wb.Worksheets.First();
            foreach (IXLRow row in ws.Rows())
            {
                Contestent contestent = new Contestent();
                contestent.Name = row.Cell("A").GetString();
                if (firstrun)
                {
                    contestent.Tornaments = 0;
                    contestent.TournementWins = 0;
                    contestent.Wins = 0;
                    contestent.Losses = 0;
                    contestent.Points = 0;
                }
                else
                {
                    contestent.Tornaments = row.Cell("B").GetValue<int>();
                    contestent.TournementWins = row.Cell("G").GetValue<int>();
                    contestent.Wins = row.Cell("H").GetValue<int>();
                    contestent.Losses = row.Cell("I").GetValue<int>();
                    contestent.Points = row.Cell("J").GetValue<int>();
                }
              
                Add(contestent);
            }
        }

        

        public void Export(string openxmlpath,string sheetname,bool overwrite)
        {
            IXLWorkbook wb = null;
            IXLWorksheet ws = null;
            IEnumerable<Contestent> contestents;
            if (overwrite)
            {
                wb = new XLWorkbook(openxmlpath);
                ws = wb.Worksheet(sheetname);
                contestents = this;
            }
            else
            {
                wb = new XLWorkbook();
                ws = wb.AddWorksheet(sheetname);
                contestents = this.OrderByDescending(x => x.Points);
            }

            foreach (Contestent contestent in contestents)
            {
                IXLRow row = null;
                if (overwrite)
                {
                    row = ws.Rows().Where(x => x.Cells().First().GetString() == contestent.Name).FirstOrDefault();
                }
                else
                {
                    row = ws.LastRowUsed() == null ? ws.Row(1) : ws.LastRowUsed().RowBelow();
                }

                if (!overwrite)
                {
                    IXLCell name = row.Cell("A");
                    name.SetValue<string>(contestent.Name);
                }

                IXLCell picked = row.Cell("B");
                picked.SetValue<int>(contestent.Tornaments);

                IXLCell champion = row.Cell("G");
                champion.SetValue<int> (contestent.TournementWins);

                IXLCell wins = row.Cell("H");
                wins.SetValue<int>(contestent.Wins);

                IXLCell losses = row.Cell("I");
                losses.SetValue<int>(contestent.Losses);

                IXLCell points = row.Cell("J");
                points.SetValue<int>(contestent.Points);

            }
            if (overwrite)
            {
                wb.Save();
            }
            else
            {
                wb.SaveAs(openxmlpath);
            }
        }
    }
}
