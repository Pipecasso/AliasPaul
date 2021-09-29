using Intergraph.UX.WPF.Toolkit;
using IsogenReportPreview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Dynamic;

namespace IsogenReportPreview.ViewModels
{
    public class IsogenExcelReportViewModel : BindableBase
    {
        private IsogenExcelReport _isogenExcelReport;
        private ObservableCollection<IsogenExcelColumn> _gridViewColumns;
        private ObservableCollection<ExpandoObject> _gridViewRows;

        public IsogenExcelReportViewModel()
        {
            _isogenExcelReport = new IsogenExcelReport();
        }
    
        public void SetPath(string path)
        {
            _isogenExcelReport.ParseSpreadsheet(path);
            _gridViewColumns = new ObservableCollection<IsogenExcelColumn>();
           /* IEnumerable<IsogenExcelRow> isogenExcelRows = _isogenExcelReport.GetRows();
            foreach (IsogenExcelRow excelRow in isogenExcelRows)
            {
                _gridViewColumns.Add(excelRow);
            }*/



           for (int i = 0; i < _isogenExcelReport.ColumnCount; i++)
           {
              IsogenExcelRow isogenExcelRow = _isogenExcelReport[i];
                _gridViewColumns.Add(_isogenExcelReport[i]);
           }
            /*_gridViewRows = new ObservableCollection<ExpandoObject>();

            for (int i=0;i<_isogenExcelReport.RownCount;i++)
            {
                for (int j=0;j<_isogenExcelReport.ColumnCount;j++)
                {
                    var item = new ExpandoObject() as IDictionary<string, object>;
                    string colname = _isogenExcelReport[j].name;
                    string test = _isogenExcelReport[j, i];
                    item.Add(_isogenExcelReport[j].name,_isogenExcelReport[j,i]);
                    _gridViewRows.Add((ExpandoObject)item);
                }
            }*/
        }

        public IEnumerable<string> GetColumnHeaders()
        {
            List<string> headerout = new List<string>();
            for (int i = 0; i < _isogenExcelReport.ColumnCount; i++)
            {
                headerout.Add(_isogenExcelReport[i].name);
            }
            return headerout;

        }
      

        public ObservableCollection<ExpandoObject> GridViewColumns2
        {
            get
            {
                //bind them as rows not columns!!!
                ObservableCollection<ExpandoObject> togo = new ObservableCollection<ExpandoObject>();
                IEnumerable<IsogenExcelRow> rows = _isogenExcelReport.GetRows();
                foreach (IsogenExcelRow row in rows)
                {
                    ExpandoObject eo = row.BindMe;
                    togo.Add(eo);
                }
                
             /*   for (int i = 0; i < _isogenExcelReport.ColumnCount; i++)
                {
                    ExpandoObject eo = _isogenExcelReport[i].BindMe;
                    togo.Add(eo);
                }*/
                return togo;
            }

        }

        public ObservableCollection<IsogenExcelColumn> GridViewColumns { get => _gridViewColumns; }
        
     /*   public ObservableCollection<ExpandoObject> GridViewRowCollection { get => _gridViewRows; }

        public ObservableCollection<ExpandoObject> DataCollection
        {
            get
            {
                return this.GridViewRowCollection;
            }
        }*/

    }
}
