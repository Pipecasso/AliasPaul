using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using WorldCupEngine;

namespace WpfApp1.ViewModel
{
    public class MatchViewModel : INotifyPropertyChanged
    {
        private Match _match;
        private Match.Result _temp_result;
        private IRelayCommand _radioCommand;
        private IRelayCommand _playCommand;
        private IRelayCommand _nextRoundCommand;
        private IRelayCommand _saveTournamentCommand;
        private Brush _brush1;
        private Brush _brush2;

        public event PropertyChangedEventHandler PropertyChanged;

        public MatchViewModel(Match match, IRelayCommand nextRoundCommand,IRelayCommand saveTournamentCommand)
        {
            _match = match;
            _temp_result = Match.Result.notplayed;
            _radioCommand = new RelayCommand<Object>(ExecuteRadio, ToPlay);
            _playCommand = new RelayCommand(Play, InPlay);
            _nextRoundCommand = nextRoundCommand;
            _saveTournamentCommand = saveTournamentCommand;
            _brush1 = new SolidColorBrush(GetPlayerColor(true));
            _brush2 = new SolidColorBrush(GetPlayerColor(false));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Contestent1 { get => _match==null ? string.Empty : $"{_match.Item1.Seeding} {_match.Item1.Name} ({_match.Item1.Tornaments})"; }
        public string Contestent2 { get => _match == null ? string.Empty : $"{_match.Item2.Seeding} {_match.Item2.Name} ({_match.Item2.Tornaments})"; }

        public Match.Result CurrentScore 
        {
            get => _temp_result; 
            set 
            { 
               _temp_result = value;
                _playCommand.NotifyCanExecuteChanged();
            }
        }

        private void ExecuteRadio(object parameter)
        {
            string who = parameter.ToString();
            CurrentScore = who == "radio_1" ? Match.Result.firstw : Match.Result.secondw;
    
        }

        private void Play()
        {
            _match.result = _temp_result;
            _playCommand.NotifyCanExecuteChanged();
            _radioCommand.NotifyCanExecuteChanged();
            _nextRoundCommand.NotifyCanExecuteChanged();
            _saveTournamentCommand.NotifyCanExecuteChanged();
            _brush1 = new SolidColorBrush(GetPlayerColor(true));
            _brush2 = new SolidColorBrush(GetPlayerColor(false));
            OnPropertyChanged("Brush1");
            OnPropertyChanged("Brush2");
        }

        private bool ToPlay(Object o)
        {
            return _match.result == Match.Result.notplayed;
        }

        public bool InPlay()
        {
            return _match.result == Match.Result.notplayed && (_temp_result == Match.Result.firstw || _temp_result == Match.Result.secondw);
        }

        public Brush Brush1
        {
            get => _brush1;
        }

        public Brush Brush2
        {
            get => _brush2;
        }

        public ICommand RadioCommand { get => _radioCommand; }
        public ICommand PlayCommand { get=> _playCommand; }

        private Color GetPlayerColor(bool player1)
        {
            Color color;
            switch (_match.result)
            { 
                case Match.Result.firstw:color = player1? Colors.ForestGreen : Colors.Tomato; break;
                case Match.Result.secondw:color = player1? Colors.Tomato : Colors.ForestGreen; break;
                case Match.Result.notplayed:
                default:
                    color = Colors.Black; break;
            }
            return color;

        }


    }
}
