using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AutoSrpintReview
{
    class Program
    {
        static void Main(string[] args)
        {
            string config = args[0];
            string outtpath = args[1];
            SR_Config sR_Config = new SR_Config(config);

            AutoSprintReview autoSprintReview = new AutoSprintReview(sR_Config);
            autoSprintReview.MakeIt(outtpath);
        }
    }
}
