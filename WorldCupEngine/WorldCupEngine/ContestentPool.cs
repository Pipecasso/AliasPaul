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

        public void Export(string openxmlpath,string sheetname)
        {
            XLWorkbook wb = new XLWorkbook(openxmlpath);
            IXLWorksheet ws = wb.Worksheets.Where(x => x.Name == sheetname).FirstOrDefault();
            foreach (Contestent contestent in this)
            {
                IXLRow row = ws.Rows().Where(x => x.Cells().First().GetString() == contestent.Name).FirstOrDefault();
                IXLCell picked = row.Cell("B");
                picked.Value = contestent.Tornaments;

                IXLCell champion = row.Cell("G");
                champion.Value = contestent.TournementWins;

                IXLCell wins = row.Cell("H");
                wins.Value = contestent.Wins;

                IXLCell losses = row.Cell("I");
                losses.Value = contestent.Losses;

                IXLCell points = row.Cell("J");
                points.Value = contestent.Points;

            }
            wb.Save();
        }
    }
}
