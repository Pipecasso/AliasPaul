using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsogenReportPreview.Models
{
    public class IsogenExcelColumn
    {
        List<string> _Cells;

        public  string name { get; set; }
       

        public IsogenExcelColumn(string colname)
        {
          
            _Cells = new List<string>();
            name = colname;
        }
        
        public string this[int i]
        {
            get
            {
                return _Cells[i];
            }
        }

        public void AddString(string s)
        {
            _Cells.Add(s);
        }

        public int CellCount
        {
            get
            {
                return _Cells.Count;
            }
        }

        public ObservableCollection<string> GridData
        {
            get
            {
                ObservableCollection<string> stringcollection = new ObservableCollection<string>();
                for (int index = 0; index < CellCount; index++)
                {
                    stringcollection.Add(this[index]);
                }
                return stringcollection;
            }
        }
    }
}
