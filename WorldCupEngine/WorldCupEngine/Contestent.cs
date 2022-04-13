using System;
using System.Collections.Generic;

namespace WorldCupEngine
{
    public class Contestent
    {
        private int _points;
        private int _tournaments;
        private int _wins;
        private int _losses;
        
        private int _tournamentwins;

        
        public string Name { get; set; }
  
        public int Points { get => _points; set { _points = value; } }
        public int Tornaments { get => _tournaments; set { _tournaments = value; } }
        public int Wins { get => _wins;set { _wins = value; } }
        public int Losses { get => _losses;set { _losses = value; } }
        public int TournementWins { get => _tournamentwins; set { _tournamentwins = value; } }

        public Contestent()
        {
            _points = 0;
            _tournaments = 0;
            _wins = 0;
            _losses = 0;
            _tournamentwins = 0;
          
        }

        public void AddPoints(int p) { _points += p; }
        public void IncWin() { _wins++; }
        public void IncLoss() { _losses++; }
        public void Picked() { _tournaments++; }
        public void TournamentWin() { _tournamentwins++; }
        
    
    }
}
