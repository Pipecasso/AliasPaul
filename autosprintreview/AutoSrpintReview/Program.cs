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
            string document = args[0];
            string templatepath = args[1];

            AutoSprintReview autoSprintReview = new AutoSprintReview(document, templatepath,"Marauders", @"\\ingrnet\eu\RUN\Users\Marauders\Sprint Review\Screenshots\");
            autoSprintReview.MakeIt("arse.pptx");
        }
    }
}
