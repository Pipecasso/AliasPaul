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
    public class CubeToFB : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double fb = 0;
            if (value != null)
            {
                CubeView cubeView = (CubeView)value;
                fb = cubeView.FrontBackDistance();
            }
            return fb;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
