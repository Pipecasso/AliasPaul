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
            _MainWindowVm.Refresh += _MainWindowVm_Refresh;
        }

        private void _MainWindowVm_Refresh(object sender, EventArgs e)
        {
            _ProgressBar.UpdateLayout();
        }
    }
}
