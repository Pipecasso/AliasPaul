using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using AliasGeometry;

namespace ProjecterSetup.ViewModels.ValueConverters
{
    public class CubeToUD : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double ud = 0;
            if (value!=null)
            {
                CubeView cubeView = (CubeView)value;
                ud = cubeView.TopBottomDistance();
            }
            return ud;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
