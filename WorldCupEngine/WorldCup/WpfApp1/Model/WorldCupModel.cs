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
        
     
        internal void Reload(string path,Tournament.Format format,int totalrounds)
        {
            _xlpath = path; 
            _contestents = new ContestentPool(path,String.Empty);
            _tournament = new Tournament(_contestents,totalrounds,format);
        }

        internal void Load(string tournamentpath,string contestentpath)
        {
            _xlpath = contestentpath;
            _contestents = new ContestentPool(contestentpath,String.Empty);
            _tournament = new Tournament(_contestents);
            _tournament.Load(tournamentpath);
        }
        
        void Save(string xlpath)
        {
            _tournament.Save(xlpath);
        }

        internal void SaveResult()
        {
            _tournament.Update(_xlpath, String.Empty);
        }
        
        internal Tournament Tournament { get => _tournament; }
        internal Round CurrentRound { get => _tournament==null ? null : _tournament.CurrentRound; }
       

    
    
    }
}
