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
using IsogenReportPreview.Converters;
using IsogenReportPreview.ViewModels;

namespace IsogenReportPreview.View
{
    /// <summary>
    /// Interaction logic for IsogenReportPreviewColumnView.xaml
    /// </summary>
    public partial class IsogenReportPreviewColumnView : UserControl
    {
        private int _rowcount;

        public IsogenReportPreviewColumnView()
        {
            InitializeComponent();
        }


        private void AddRows()
        {
            for (int i=0;i<_rowcount;i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                ValueGrid.RowDefinitions.Add(rowDefinition);
            }

            for (int i = 0; i < _rowcount; i++)
            {
                string name = $"Row{i}";
                string bindkey = $"item{i}";
                Label cell = new Label();
                cell.Name = name;
               
                Binding bind = new Binding("BindMe");
                bind.Converter = new ColumnValueConverter();
                bind.ConverterParameter = bindkey;
                cell.SetBinding(Label.ContentProperty, bind);

                Grid.SetRow(cell, i);
                Grid.SetColumn(cell, 0);
                cell.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                cell.BorderThickness = new Thickness(1, i == 0 ? 1 : 0, 1, 1);
                ValueGrid.Children.Add(cell);
            }
       
        }

        

        public int rowcount
        { 
            get
            {
                return _rowcount;
            }
            set
            {
                _rowcount = value;
                AddRows();
            }
        
        }

    }
}
