using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCupEngine;

namespace WpfApp1.Model
{
    internal class WorldCupModel
    {
        private ContestentPool _contestents;
        private Tournament _tournament;

        internal void Reload(string path,string sheet)
        {
            _contestents = new ContestentPool(path,sheet);
            _tournament = new Tournament(_contestents);
        }

    
    
    }
}
