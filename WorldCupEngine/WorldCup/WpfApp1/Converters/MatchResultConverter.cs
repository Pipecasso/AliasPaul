using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WorldCupEngine;

namespace WpfApp1.Converters
{
    public class MatchResultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Match.Result resultin = (Match.Result)value;
            bool togo = resultin == Match.Result.firstw || resultin == Match.Result.secondw;
            return togo;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
