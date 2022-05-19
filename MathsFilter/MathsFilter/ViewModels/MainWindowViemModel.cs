using GeoFilter;
using Microsoft.Toolkit.Mvvm.Input;

using System.Windows.Input;
using System.Drawing;
using System.IO;

using System.ComponentModel;
using MathsFilter.Models;

namespace MathsFilter.ViewModels
{
    internal class MainWindowViemModel : INotifyPropertyChanged
    {
        private RelayCommand _goCommand;
        private MathsFilterModel _model;
        private string _funcString;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string FuncString
        {
            get { return _funcString; }
            set { 
                    _funcString = value;
                    _model.SetFunctionString(_funcString);
                    OnPropertyChanged(nameof(FuncString));
                    _goCommand.NotifyCanExecuteChanged();
                }
        }

        public ICommand GoCommand { get { return _goCommand; } }
                
        public MainWindowViemModel()
        {
            _model = new MathsFilterModel();
            _goCommand = new RelayCommand(Go,CanGo);
        }

        private bool CanGo()
        {
            bool result = _model.ValidFunction();
            return _model.ValidFunction();
        }

        private void Go()
        {

        }
    }
}
