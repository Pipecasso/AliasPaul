using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsogenReportPreview.Models
{
    public class IsogenExcelRow
    {
        protected List<string> _Cells;
        public IsogenExcelRow()
        {
            _Cells = new List<string>();
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

        internal int CellCount
        {
            get
            {
                return _Cells.Count;
            }
        }

        public ExpandoObject BindMe
        {
            get
            {
                var item = new ExpandoObject() as IDictionary<string, object>;
                int tick = 0;
                foreach (string s in _Cells)
                {
                    string key = $"item{tick}";
                    item.Add(key, s);
                    tick++;
                }
                return (ExpandoObject)item;
            }
        }
    }
}

