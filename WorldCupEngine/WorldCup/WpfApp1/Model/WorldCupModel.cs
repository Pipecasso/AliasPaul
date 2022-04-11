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
        private string _xlpath;
        private string _sheet;
     
        internal void Reload(string path,string sheet,int totalrounds = 5)
        {
            _xlpath = path;
            _sheet = sheet;
            
            _contestents = new ContestentPool(path,sheet);
            _tournament = new Tournament(_contestents,totalrounds,false);
        }
        
        void Save(string xlpath)
        {
            _tournament.Save(xlpath);
        }

        internal void SaveResult()
        {
            _contestents.Export(_xlpath, _sheet, true);
        }
        
        internal Tournament Tournament { get => _tournament; }
        internal Round CurrentRound { get => _tournament==null ? null : _tournament.CurrentRound; }
       

    
    
    }
}
