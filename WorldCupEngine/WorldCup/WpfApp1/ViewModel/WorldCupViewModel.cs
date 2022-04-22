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
        private RelayCommand _NewTournamentCommand;
        private string _xlspath;
    
        public WorldCupViewModel()
        {
            _worldCupModel = new WorldCupModel();
            _RoundControls = new Dictionary<Round, List<MatchControl>>();
            _RoundCompleteCommand = new RelayCommand(NextRound,RoundComplete);
            _NewTournamentCommand = new RelayCommand(NewTournament, CanHaveNewTournament);
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

        public void NewTournament()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "xlsx files (*.xlsx)|*.xlsx";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                NewContestents(ofd.FileName);
            }
            RoundCompleteSignal.Invoke(this, new EventArgs());
            _RoundCompleteCommand.NotifyCanExecuteChanged();
            _NewTournamentCommand.NotifyCanExecuteChanged();
        }

        public void NewContestents(string path)
        {
            _xlspath = path;
            _worldCupModel.Reload(path);
            MakeMatchControls();
        }

        public bool RoundComplete()
        {
            return _worldCupModel.CurrentRound!=null && _worldCupModel.CurrentRound.AllMatches.All(x => x.result != Match.Result.notplayed);
        }

        public bool CanHaveNewTournament()
        {
            return _worldCupModel.Tournament == null || _worldCupModel.Tournament.Winner() != null;
        }

        public void NextRound()
        {
            if (_worldCupModel.CurrentRound.IsFinal)
            {
                Contestent winner = _worldCupModel.Tournament.Winner();
                DialogResult dr =  MessageBox.Show($"Congratulations to {winner.Name}. Play again?","Game Over!",MessageBoxButtons.YesNo);
               
                _worldCupModel.Tournament.NextRound();
                _worldCupModel.SaveResult();
                _RoundControls.Clear();
                if (dr == DialogResult.Yes)
                {
                    NewTournament();
                }
                else
                {
                
                }
            }
            else
            {
                _worldCupModel.Tournament.NextRound();
                _RoundCompleteCommand.NotifyCanExecuteChanged();
                MakeMatchControls();
            }
            RoundCompleteSignal.Invoke(this, new EventArgs());

        }

        public ICommand NextRoundCommand { get => _RoundCompleteCommand; }
        public ICommand NewTournamentCommand { get => _NewTournamentCommand; }

        public IEnumerable<MatchControl> CurrentControls
        {
            get
            {
                IEnumerable<MatchControl> controls = null;
                if (_worldCupModel.Tournament.Winner() != null)
                {
                    List<MatchControl> controls2 = new List<MatchControl>();
                    controls = controls2;
                }
                else
                {
                    controls = _RoundControls[_worldCupModel.CurrentRound];
                }
                return controls;
            }
           
        }

        public EventHandler RoundCompleteSignal { get; set; }

       

    }
}
