using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using AliasGeometry;
using System.Windows.Data;

namespace ProjecterSetup.ViewModels.ValueConverters
{
    public class CubeToLR : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double lr = 0;
            if (value != null)
            {
                CubeView cubeView = (CubeView)value;
                lr = cubeView.LeftRightDistance();
            }
            return lr;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
