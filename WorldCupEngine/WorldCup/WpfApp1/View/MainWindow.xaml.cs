using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NextRound += MainWindow_NextRound;
            CloseDown += MainWindow_Close;
            WorldCupVm.RoundCompleteSignal = NextRound;
            WorldCupVm.CloseSignal = CloseDown;
        }

        private void MainWindow_Close(object  sender,EventArgs e)
        {
            this.Close();
        }

        private void MainWindow_NextRound(object sender, EventArgs e)
        {
            ShowMatches();
        }

        public event EventHandler CloseDown;
        public event EventHandler NextRound;

        private void ShowMatches()
        {
           _stackPanel.Children.Clear();
            foreach (MatchControl matchControl in WorldCupVm.CurrentControls)
            { _stackPanel.Children.Add(matchControl); }
        }
    }
}
