using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IsogenReportPreview.Models;

namespace IsogenReportPreview.ViewModels
{
    public class IsogenReportPreviewColumnViewModel
    {
        private IsogenExcelColumn isogenExcelColumn;
        dynamic _Properties;

        public IsogenExcelColumn IsogenExcelColumn
        {
            get
            {
                return isogenExcelColumn;
            }
            set
            {
                isogenExcelColumn = value;
                AddDynamicProperties();

            }
        }

        private void AddDynamicProperties()
        {
            _Properties = new ExpandoObject();
            int tick = 0;
            foreach (string s in isogenExcelColumn)
            {
                string key = $"item{tick}";
                ((IDictionary<string, object>)_Properties).Add(key, s);
                tick++;
            }
        }

        public string HeaderName
        {
            get
            {
                string retstring = string.Empty;
                if (isogenExcelColumn!=null)
                {
                    retstring = isogenExcelColumn.name;
                }
                return retstring;
            }
        }

        public ExpandoObject BindMe
        {
            get
            { 
                return (ExpandoObject)_Properties;
            }
        }
    }
}
