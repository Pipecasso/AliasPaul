using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace AutoSrpintReview
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Write("Please pass two arguments. The xml config file and the output powerpoint");
            }
            else
            {
                string config = args[0];
                string outtpath = args[1];
                if (!File.Exists(config))
                {
                    Console.Write($"cannot find - {config} ");
                }
                else
                {
                    SR_Config sR_Config = new SR_Config(config);

                    AutoSprintReview autoSprintReview = new AutoSprintReview(sR_Config);
                    autoSprintReview.MakeIt(outtpath);
                }
            }
        }
    }
}
