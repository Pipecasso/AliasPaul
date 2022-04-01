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

namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for RoundControl.xaml
    /// </summary>
    public partial class RoundControl : UserControl
    {
        public RoundControl()
        {
            InitializeComponent();
        }

        public void DeployMatchControl(IEnumerable<MatchControl> matchControls)
        {
            foreach (MatchControl matchControl in matchControls)
            {
                MatchStack.Children.Add(matchControl);
            }
        }
        
       
    }
}
