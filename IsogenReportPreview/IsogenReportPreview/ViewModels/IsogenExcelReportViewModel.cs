using Intergraph.UX.WPF.Toolkit;
using IsogenReportPreview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace IsogenReportPreview.ViewModels
{
    public class IsogenExcelReportViewModel : BindableBase
    {
        private IsogenExcelReport _isogenExcelReport;

        public IsogenExcelReportViewModel()
        {
            _isogenExcelReport = new IsogenExcelReport();
        }
    
        public void SetPath(string path)
        {
            _isogenExcelReport.ParseSpreadsheet(path);
        }

        internal IEnumerable<string> GetColumns()
        {
            List<string> columnnames = new List<string>();
            for (int i=0;i<_isogenExcelReport.ColumnCount;i++)
            {
                columnnames.Add(_isogenExcelReport[i].name);
            }
            return columnnames;
        }

        public ObservableCollection<IsogenExcelColumn> GetColumns2
        {
            get
            {
                ObservableCollection<IsogenExcelColumn> columnnames = new ObservableCollection<IsogenExcelColumn>();
                for (int i = 0; i < _isogenExcelReport.ColumnCount; i++)
                {
                    columnnames.Add(_isogenExcelReport[i]);
                }
                return columnnames;
            }
        }

        public ObservableCollection<string> GridData(int i)
        {
            ObservableCollection<string> stringcollection = new ObservableCollection<string>();
            IsogenExcelColumn isogenExcelColumn = _isogenExcelReport[i];
            for (int index = 0;index < isogenExcelColumn.CellCount;index++)
            {
                stringcollection.Add(isogenExcelColumn[index]);
            }
            return stringcollection;
        }
    }
}
