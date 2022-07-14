using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WorldCupEngine;

namespace WpfApp1.Converters
{
    public class TournamentTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool check = false;
            if (value != null)
            {
                Tournament.Format format = (Tournament.Format)value;
                string sparameter = parameter as string;

                if (value == null)
                {
                    check = false;
                }
                else
                {
                    switch (format)
                    {
                        case Tournament.Format.standard: check = sparameter == "ttstandard"; break;
                        case Tournament.Format.facup: check = sparameter == "ttfacup"; break;
                        case Tournament.Format.seeded: check = sparameter == "ttseeded"; break;
                    }
                }
            }
            return check;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string sparameter = parameter as string;
            object objret = null;
            Tournament.Format format;
            if (System.Convert.ToBoolean(value))
            {
                switch (sparameter)
                {
                    case "ttfacup": format = Tournament.Format.facup; break;
                    case "ttseeded": format = Tournament.Format.seeded; break;
                    case "ttstandard":
                    default:
                        format = Tournament.Format.standard; break;
                }
                objret = format;
            }
            return objret;
        }

    }
}
