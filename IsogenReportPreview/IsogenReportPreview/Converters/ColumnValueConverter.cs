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
        private int _tick;

        public ColumnValueConverter()
        {
            _tick = 0;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object objgo;
            string key = $"item{_tick}";
            var row = value as IDictionary<string, object>;
            if (row.ContainsKey(key))
            {
                objgo = row[key];
            }
            else
            {
                objgo = "cheesepie";
            }
            _tick++;
            return objgo;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
