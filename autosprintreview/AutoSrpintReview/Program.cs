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
            string powerpath = args[1];
            string templatepath = args[2];

            AutoSprintReview autoSprintReview = new AutoSprintReview(document, templatepath);
            autoSprintReview.MakeIt();

         

           

           
        }
    }
}
