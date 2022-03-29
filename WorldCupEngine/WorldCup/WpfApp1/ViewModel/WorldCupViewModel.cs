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
        private ICommand _reloadommand;
        private Dictionary<Round, List<MatchControl>> _RoundControls;


        public WorldCupViewModel()
        {
            _worldCupModel = new WorldCupModel();
            _reloadommand = new RelayCommand(NewContestents);
            _RoundControls = new Dictionary<Round, List<MatchControl>>();
        }

        public void NewContestents()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "xlsx files (*.xlsx)|*.xlsx";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _worldCupModel.Reload(ofd.FileName, "");
            }

            List<MatchControl> FirstControl = new List<MatchControl>();
            foreach (Match m in _worldCupModel.CurrentRound.AllMatches)
            {
                MatchControl matchControl = new MatchControl();
                MatchViewModel matchViewModel = new MatchViewModel(m);
                matchControl.DataContext = matchViewModel;
                FirstControl.Add(matchControl);
            }
            _RoundControls.Add(_worldCupModel.CurrentRound,FirstControl);
            //now tell our dude he's got controls.
        }
        public ICommand ReloadCommand
        {
            get => _reloadommand;
        }

        public IEnumerable<MatchControl> CurrentControls { get => _RoundControls[_worldCupModel.CurrentRound]; }
        

        
    }
}
