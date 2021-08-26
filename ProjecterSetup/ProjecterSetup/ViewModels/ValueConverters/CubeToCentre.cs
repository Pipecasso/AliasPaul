using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using AliasGeometry;
using System.Globalization;

namespace ProjecterSetup.ViewModels.ValueConverters
{
    public class CubeToCentre : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Centre = string.Empty;
            if (value != null)
            {
                CubeView cubeView = (CubeView)(value);
                Centre = cubeView.CenterDisplay;
            }

            return Centre;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
