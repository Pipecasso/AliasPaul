using GeoFilter;
using Microsoft.Toolkit.Mvvm.Input;

using System.Windows.Input;
using System.Drawing;
using System.IO;

using System.ComponentModel;
using MathsFilter.Models;
using System;

namespace MathsFilter.ViewModels
{
    internal class MainWindowViemModel : INotifyPropertyChanged
    {
        private RelayCommand _goCommand;
        private MathsFilterModel _model;
        private string _funcString;
        private double _Progress;



        public event EventHandler Refresh;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string FuncString
        {
            get { return _funcString; }
            set
            {
                _funcString = value;
                _model.SetFunctionString(_funcString);
                OnPropertyChanged(nameof(FuncString));
                _goCommand.NotifyCanExecuteChanged();
                Progress = 0;
            }
        }

        public ICommand GoCommand { get { return _goCommand; } }

        public MainWindowViemModel()
        {
            _model = new MathsFilterModel();
            _goCommand = new RelayCommand(Go, CanGo);
        }



        private bool CanGo()
        {
            return _model.ValidFunction();
        }

        private void Go()
        {
            _model.InitialiseTransformMatrix(700);
            _model.TransformMatrix.Pulse += TransformMatrix_Pulse;
            _model.TransformMatrix.Set(_model.MainFunction);

        }

        private void TransformMatrix_Pulse(object sender, System.EventArgs e)
        {
            PulseArgs pulseArgs = e as PulseArgs;
            Progress = pulseArgs.PercentComplete();
        }

        public double Progress
        {
            get
            {
                return _Progress;
            }
            set
            {
                _Progress = value;
                OnPropertyChanged(nameof(Progress));
                Refresh?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
