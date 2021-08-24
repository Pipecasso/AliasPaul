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
using Microsoft.Win32;
using PodToPoints;
using AliasGeometry;
using ProjecterSetup.Views;
using ProjecterSetup.ViewModels;
using ProjecterSetup.Models;

namespace ProjecterSetup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _manfiest = @"D:\AliasPaul\bin\Debug\I-Configure.exe.manifest";
    
        
        public MainWindow()
        {
            InitializeComponent();
        }

       

        private void LoadPod_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //openFileDialog.InitialDirectory = "c:\\";
            ofd.Filter = "pod files (*.pod)|*.pod";
            ofd.RestoreDirectory = true;
            bool? yes = ofd.ShowDialog();
            if (yes.HasValue && yes==true)
            {
                PODTransformer pODTransformer = new PODTransformer(_manfiest, ofd.FileName);
                ProjectorViewWindow projectorViewWindow = new ProjectorViewWindow();
                ProjectorViewModel projectorViewModel = (ProjectorViewModel)projectorViewWindow.DataContext;
                ProjectorModel projectorModel = projectorViewModel.projectorModel;

                projectorModel.Cube = pODTransformer.GetCube();
                projectorViewWindow.Show();





            }



        }
    }
}
