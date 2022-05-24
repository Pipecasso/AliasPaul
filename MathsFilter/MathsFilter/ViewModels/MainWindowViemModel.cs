using GeoFilter;
using Microsoft.Toolkit.Mvvm.Input;

using System.Windows.Input;
using System.Drawing;
using System.IO;

using System.ComponentModel;
using MathsFilter.Models;
using System;
using MathsFilter.Views;
using System.Windows.Media.Imaging;

namespace MathsFilter.ViewModels
{
    internal class MainWindowViemModel : INotifyPropertyChanged
    {
        private RelayCommand _goCommand;
        private RelayCommand _analyseCommand;
        private RelayCommand _showCommand;
        private MathsFilterModel _model;
        private string _funcString;
        private double _Progress;
        private BackgroundWorker _worker;
        private bool _hasCalculculated;
        private bool _isBusy;
        
    
        public event PropertyChangedEventHandler PropertyChanged;
       
        public MainWindowViemModel()
        {
            _model = new MathsFilterModel();
            _goCommand = new RelayCommand(Go, CanGo);
            _analyseCommand = new RelayCommand(AnalyseMatrix, MatrixSet);
            _showCommand = new RelayCommand(Show, MatrixSet);
            _worker = new BackgroundWorker();
            _hasCalculculated = false;
            _analyseCommand.NotifyCanExecuteChanged();
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
            _isBusy = false;
        }

      

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
        public ICommand AnalyseCommand { get { return _analyseCommand;} }
        public ICommand ShowCommand { get { return _showCommand; } }
       
        private bool CanGo()
        {
            return _model.ValidFunction() && !_isBusy;
        }

        private bool MatrixSet()
        {
            return _hasCalculculated;
        }

        private void AnalyseMatrix()
        {
            AnalysisViewModel analysisViewModel = new AnalysisViewModel(_model.TransformMatrix);
            AnalysisWindow analysisWindow = new AnalysisWindow();
            analysisWindow.DataContext = analysisViewModel;
            analysisWindow.Show();
        }

        public int XOffset
        {
            get { return _model.XOffset; }
            set
            {
                _model.XOffset = value;
                OnPropertyChanged(nameof(XOffset));
            }
        }

        public int YOffset
        {
            get { return _model.YOffset; }
            set
            {
                _model.XOffset = value;
                OnPropertyChanged(nameof(YOffset));
            }
        }

        public double Scale
        {
            get { return _model.Scale; }
            set
            {
                _model.Scale = value;
                _model.SetFunctionString(_funcString);
                OnPropertyChanged(nameof(Scale));
                _goCommand?.NotifyCanExecuteChanged();
            }
        }

        private void Go()
        {
            _hasCalculculated = false;
            _model.InitialiseTransformMatrix(700);
            _model.TransformMatrix.Pulse += TransformMatrix_Pulse;
            _isBusy = true;
            _goCommand?.NotifyCanExecuteChanged();
            _worker.RunWorkerAsync();
        }

        private void Show()
        {
            _model.PaintImage();
            OnPropertyChanged(nameof(Image));
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _model.TransformMatrix.Set(_model.MainFunction);
         
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _hasCalculculated = true;
            _isBusy = false;
            _analyseCommand?.NotifyCanExecuteChanged();
            _showCommand?.NotifyCanExecuteChanged();
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
               
            }
        }

        public BitmapImage Image { get { return _model.Image; } }
    }
}
