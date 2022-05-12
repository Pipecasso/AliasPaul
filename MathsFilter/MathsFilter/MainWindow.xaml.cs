using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using GeoFilter;
using org.mariuszgromada.math.mxparser;
using System.Drawing;
using System.IO;

namespace MathsFilter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Function func = new Function("f(x,y) = sqrt(x^2 + y^2)");

            Grid grid = (Grid)_bimage.Parent;
            int row = Grid.GetRow(_bimage);
            int column = Grid.GetColumn(_bimage);
            RowDefinition rowDefinition = grid.RowDefinitions[row];
            ColumnDefinition columnDefinition = grid.ColumnDefinitions[column];

            int range = Convert.ToInt16(columnDefinition.ActualWidth < rowDefinition.ActualHeight ? columnDefinition.ActualWidth : rowDefinition.ActualHeight);
         
            TransformMatrix tm = new TransformMatrix(range/2, 0);
            tm.Set(func);

            int size = range * 2 + 1;
            BitmapBox bitbox = new BitmapBox(System.Drawing.Color.Gray, size, size);
            BitmapBox.OutOfBounds oob = BitmapBox.OutOfBounds.Rollover;
            bitbox.ApplyMatrix(tm, size / 2, size / 2, BitmapBox.Colour.Red, oob);


          
      
            MemoryStream ms = new MemoryStream();
            bitbox.bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            _bimage.Source = bi;   
        }
    }
}
