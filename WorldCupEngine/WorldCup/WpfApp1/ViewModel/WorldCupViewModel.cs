using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using WpfApp1.Model;
using System.Windows.Forms;
using WorldCupEngine;
using WpfApp1.View;


namespace WpfApp1.ViewModel
{
    public class WorldCupViewModel 
    {
        private WorldCupModel _worldCupModel;
    
        private Dictionary<Round, List<MatchControl>> _RoundControls;
        private RelayCommand _RoundCompleteCommand;
        private string _xlspath;
    
        public WorldCupViewModel()
        {
            _worldCupModel = new WorldCupModel();
            _RoundControls = new Dictionary<Round, List<MatchControl>>();
            _RoundCompleteCommand = new RelayCommand(NextRound,RoundComplete);
        }

        private void MakeMatchControls()
        {
            List<MatchControl> FirstControl = new List<MatchControl>();
            foreach (Match m in _worldCupModel.CurrentRound.AllMatches)
            {
                MatchControl matchControl = new MatchControl();
                MatchViewModel matchViewModel = new MatchViewModel(m, _RoundCompleteCommand);
                matchControl.DataContext = matchViewModel;
                FirstControl.Add(matchControl);
            }
            _RoundControls.Add(_worldCupModel.CurrentRound, FirstControl);
        }

        public void NewContestents(string path)
        {
            _xlspath = path;
            _worldCupModel.Reload(path, "");
            MakeMatchControls();
        }

        public bool RoundComplete()
        {
            return _worldCupModel.CurrentRound!=null && _worldCupModel.CurrentRound.AllMatches.All(x => x.result != Match.Result.notplayed);
        }

        public void NextRound()
        {
            if (_worldCupModel.CurrentRound.IsFinal)
            {
                Contestent winner = _worldCupModel.Tournament.Winner();
                MessageBox.Show($"Congratulations to {winner.Name}");
                _worldCupModel.Tournament.NextRound();
            }
            else
            {
                _worldCupModel.Tournament.NextRound();
                _RoundCompleteCommand.NotifyCanExecuteChanged();
                MakeMatchControls();
                RoundCompleteSignal.Invoke(this, new EventArgs());
            }
           
        }

        public ICommand NextRoundCommand { get => _RoundCompleteCommand; }

        public IEnumerable<MatchControl> CurrentControls { get => _RoundControls[_worldCupModel.CurrentRound]; }

        public EventHandler RoundCompleteSignal { get; set; }

       

    }
}
