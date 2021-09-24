using IsogenReportPreview.ViewModels;
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

namespace IsogenReportPreview.View
{
    /// <summary>
    /// Interaction logic for IsogenReportPreviewView.xaml
    /// </summary>
    public partial class IsogenReportPreviewView : UserControl
    {
        public IsogenReportPreviewView()
        {
            InitializeComponent();
        }

        public void SetPath(string path)
        {
            IsogenExcelReportViewModel isogenExcelReportViewModel = (IsogenExcelReportViewModel)this.DataContext;
            isogenExcelReportViewModel.SetPath(path);
            int tick = 0;
            if (isogenExcelReportViewModel != null)
            {
                foreach (string s in isogenExcelReportViewModel.GetColumns())
                {
                    DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
             
                    dataGridTextColumn.Header = s;
                    Binding binding = new Binding("GridData");
                   
                    dataGridTextColumn.Binding = binding;
                    preview_grid.Columns.Add(dataGridTextColumn);
                    tick++;
                }
            
            }
        }
    }
}
