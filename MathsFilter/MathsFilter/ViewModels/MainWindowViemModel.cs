using GeoFilter;
using Microsoft.Toolkit.Mvvm.Input;

using System.Windows.Input;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ComponentModel;
using MathsFilter.Models;
using System;
using MathsFilter.Views;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;

namespace MathsFilter.ViewModels
{
    internal class MainWindowViemModel : INotifyPropertyChanged
    {
        private RelayCommand _goCommand;
        private RelayCommand _analyseCommand;
        private RelayCommand _showCommand;
        private RelayCommand _saveCommand;
        private MathsFilterModel _model;
        private string _funcString;
        private double _Progress;
        private BackgroundWorker _worker;
        private bool _hasCalculculated;
        private bool _isBusy;
        private bool _painted;
        private int _range;
        uint _gap;


        public event PropertyChangedEventHandler PropertyChanged;
       
        public MainWindowViemModel()
        {
            _range = 1000;
            _gap = 1;
            _model = new MathsFilterModel();
            _goCommand = new RelayCommand(Go, CanGo);
            _analyseCommand = new RelayCommand(AnalyseMatrix, MatrixSet);
            _showCommand = new RelayCommand(Show, MatrixSet);
            _saveCommand = new RelayCommand(Save, CanSave);
            _worker = new BackgroundWorker();
            _hasCalculculated = false;
            _analyseCommand.NotifyCanExecuteChanged();
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
            _isBusy = false;
            _painted = false;
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

        public ICommand SaveCommand { get { return _saveCommand; } }    
       
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
                _model.SetFunctionString(_funcString);
                OnPropertyChanged(nameof(XOffset));
                _goCommand?.NotifyCanExecuteChanged();
            }
        }

        public int YOffset
        {
            get { return _model.YOffset; }
            set
            {
                _model.YOffset = value;    
                _model.SetFunctionString(_funcString);
                OnPropertyChanged(nameof(YOffset));
                _goCommand?.NotifyCanExecuteChanged();
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

        public int Range
        {
            get { return _range; }
            set { _range = value; OnPropertyChanged(nameof(Range)); }
        }

        public uint Gap
        {
            get { return _gap; }
            set { _gap = value; OnPropertyChanged(nameof(Gap)); }
        }


        private void Go()
        {
            _hasCalculculated = false;
            _painted = false;
            _model.InitialiseTransformMatrix(_range,_gap);
            _model.TransformMatrix.Pulse += TransformMatrix_Pulse;
            _isBusy = true;
            _goCommand?.NotifyCanExecuteChanged();
            _saveCommand?.NotifyCanExecuteChanged();
            _worker.RunWorkerAsync();
         



        }

    private void Show()
        {
            _model.PaintImage();
            OnPropertyChanged(nameof(Image));
            _painted = true;
            _saveCommand.NotifyCanExecuteChanged();
        }

        private void Save()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "bmp files (*.bmp)|*.bmp";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _model.SaveBox(saveFileDialog.FileName);
            }
        }

        private bool CanSave()
        {
            return _painted;
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

            ChunkyIntList chunks = _model.TransformMatrix.SortedValues;
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
