using GeoFilter;
using Microsoft.Toolkit.Mvvm.Input;

using System.Windows.Input;
using System.Drawing;
using System.IO;

using System.ComponentModel;
using MathsFilter.Models;

namespace MathsFilter.ViewModels
{
    internal class MainWindowViemModel
    {
        private RelayCommand _goCommand;
        private MathsFilterModel _model;

        public string FuncString { get; set; }

        public IRelayCommand GoCommand { get { return _goCommand; } }
                
        public MainWindowViemModel()
        {
            _model = new MathsFilterModel();
            _goCommand = new RelayCommand(Go,CanGo);
        }

        private bool CanGo()
        {
            return _model.ValidFunction();
        }

        private void Go()
        {

        }
    }
}
