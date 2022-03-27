using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using WpfApp1.Commands;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    internal class WorldCupViewModel
    {
        private WorldCupModel _worldCupModel;
        private ICommand _reloadommand;
        
        internal WorldCupViewModel()
        {
            _worldCupModel = new WorldCupModel();
            _reloadommand = new ReloadCommand();
       
        }

        internal ICommand ReloadCommand
        {
            get => _reloadommand;
        }

      
    
    }
}
