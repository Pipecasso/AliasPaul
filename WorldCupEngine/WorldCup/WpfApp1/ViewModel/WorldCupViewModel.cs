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
   
        public WorldCupViewModel()
        {
            _worldCupModel = new WorldCupModel();
            _RoundControls = new Dictionary<Round, List<MatchControl>>();
        }

        public void NewContestents(string path)
        {
            _worldCupModel.Reload(path, "");
            List<MatchControl> FirstControl = new List<MatchControl>();
            foreach (Match m in _worldCupModel.CurrentRound.AllMatches)
            {
                MatchControl matchControl = new MatchControl();
                MatchViewModel matchViewModel = new MatchViewModel(m);
                matchControl.DataContext = matchViewModel;
                FirstControl.Add(matchControl);
            }
            _RoundControls.Add(_worldCupModel.CurrentRound, FirstControl);
        }

    

        public IEnumerable<MatchControl> CurrentControls { get => _RoundControls[_worldCupModel.CurrentRound]; }
        

        
    }
}
