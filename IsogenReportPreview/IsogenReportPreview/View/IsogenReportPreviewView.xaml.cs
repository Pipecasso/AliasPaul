
using IsogenReportPreview.ViewModels;
using System.Windows.Controls;

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
            IsogenReportPreviewViewModel isogenReportPreviewViewModel = new IsogenReportPreviewViewModel();
            this.DataContext = isogenReportPreviewViewModel;
            isogenReportPreviewViewModel.SetPath(path);
            for (int i=0;i<isogenReportPreviewViewModel.ColumnCount;i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                PreviewGrid.ColumnDefinitions.Add(columnDefinition);
            }

            for (int i=0;i<isogenReportPreviewViewModel.ColumnCount;i++)
            {
                IsogenReportPreviewColumnView isogenReportPreviewColumnView = new IsogenReportPreviewColumnView();
                isogenReportPreviewColumnView.rowcount = isogenReportPreviewViewModel.RowCount;
                Grid.SetRow(isogenReportPreviewColumnView, 0);
                Grid.SetColumn(isogenReportPreviewColumnView, i);
                PreviewGrid.Children.Add(isogenReportPreviewColumnView);

                IsogenReportPreviewColumnViewModel isogenReportPreviewColumnViewModel = new IsogenReportPreviewColumnViewModel();
                isogenReportPreviewColumnView.DataContext = isogenReportPreviewColumnViewModel;
                isogenReportPreviewViewModel.AssignColumn(i, isogenReportPreviewColumnViewModel);
            }
        }
    }
}
