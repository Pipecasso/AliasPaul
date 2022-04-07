using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldCupEngine;

namespace WpfApp1.ViewModel
{
    public class MatchViewModel
    {
        private Match _match;
        private Match.Result _temp_result;
        private IRelayCommand _radioCommand;
        private IRelayCommand _playCommand;
    
        

        public MatchViewModel(Match match)
        {
            _match = match;
            _temp_result = Match.Result.notplayed;
            _radioCommand = new RelayCommand<Object>(ExecuteRadio, ToPlay);
            _playCommand = new RelayCommand(Play, InPlay);
        }

        public string Contestent1 { get => _match==null ? string.Empty : _match.Item1.Name; }
        public string Contestent2 { get => _match==null ? string.Empty : _match.Item2.Name; }

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
        }

        private bool ToPlay(Object o)
        {
            return _match.result == Match.Result.notplayed;
        }

        public bool InPlay()
        {
            return _match.result == Match.Result.notplayed && (_temp_result == Match.Result.firstw || _temp_result == Match.Result.secondw);
        }

        public ICommand RadioCommand { get => _radioCommand; }
        public ICommand PlayCommand { get=> _playCommand; }


        
    }
}
