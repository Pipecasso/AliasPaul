using IsogenReportPreview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsogenReportPreview.ViewModels
{
    public class IsogenReportPreviewViewModel
    {
        private IsogenExcelReport _isogenExcelReport;

        public void SetPath(string path)
        {
            _isogenExcelReport = new IsogenExcelReport();
            _isogenExcelReport.ParseSpreadsheet(path);
        }

        public int ColumnCount
        { 
            get
            {
                int valtogo = 0;
                if (_isogenExcelReport!=null)
                {
                    valtogo = _isogenExcelReport.ColumnCount;
                }
                return valtogo;
            }
        }

        public int RowCount
        {
            get
            {
                int valtogo = 0;
                if (_isogenExcelReport!=null)
                {
                    valtogo = _isogenExcelReport.RowCount;
                }
                return valtogo;
            }
        }

        public void AssignColumn(int index,IsogenReportPreviewColumnViewModel reportPreviewColumnViewModel)
        {
            reportPreviewColumnViewModel.IsogenExcelColumn = _isogenExcelReport[index];
        }
    }
}
