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
            WorldCupVm.RoundCompleteSignal = NextRound;
        }

        private void MainWindow_NextRound(object sender, EventArgs e)
        {
            ShowMatches();
        }

        public event EventHandler NextRound;

        private void ShowMatches()
        {
           _stackPanel.Children.Clear();
            foreach (MatchControl matchControl in WorldCupVm.CurrentControls)
            { _stackPanel.Children.Add(matchControl); }
        }

        public void NewTournament_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "xlsx files (*.xlsx)|*.xlsx";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                 WorldCupVm.NewContestents(ofd.FileName);
            }
            ShowMatches();
        }
    }
}
