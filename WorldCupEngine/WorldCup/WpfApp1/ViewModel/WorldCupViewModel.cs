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

namespace WpfApp1.ViewModel
{
    public class WorldCupViewModel
    {
        private WorldCupModel _worldCupModel;
        private ICommand _reloadommand;
        
        public WorldCupViewModel()
        {
            _worldCupModel = new WorldCupModel();
            _reloadommand = new RelayCommand(NewContestents);
        }

        public void NewContestents()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "xlsx files (*.xlsx)|*.xlsx";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _worldCupModel.Reload(ofd.FileName, "");
            }
        }
        public ICommand ReloadCommand
        {
            get => _reloadommand;
        }

        public IEnumerable<Match> Matches
        {
            get => _worldCupModel.CurrentRound.AllMatches;
        }

      
    
    }
}
