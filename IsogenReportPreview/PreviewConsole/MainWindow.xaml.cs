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
using IsogenReportPreview.ViewModels;
using IsogenReportPreview.Models;
namespace PreviewConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string path = @"D:\AliasPaul\IsogenReportPreview\IsogenReportingPreviewTests\bin\Debug\SpoolInformationFileImperial.xlsx";

            IsogenExcelReport isogenExcelModel = new IsogenExcelReport();
            isogenExcelModel.ParseSpreadsheet(path);
            IsogenExcelColumn colin = isogenExcelModel[0];
            preview2.rowcount = colin.CellCount;

            IsogenReportPreviewColumnViewModel isogenReportPreviewColumnViewModel = new IsogenReportPreviewColumnViewModel();
            preview2.DataContext = isogenReportPreviewColumnViewModel;
            isogenReportPreviewColumnViewModel.IsogenExcelColumn = colin;


        }
    }
}
