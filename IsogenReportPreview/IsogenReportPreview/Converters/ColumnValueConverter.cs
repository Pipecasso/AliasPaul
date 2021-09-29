using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IsogenReportPreview.Converters
{
    public class ColumnValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object objgo;
            string key = parameter.ToString();
            var row = value as IDictionary<string, object>;
            if (row.ContainsKey(key))
            {
                objgo = row[key];
            }
            else
            {
                objgo = "cheesepie";
            }
            return objgo;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
