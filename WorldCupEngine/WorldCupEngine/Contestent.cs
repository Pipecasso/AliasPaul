using System;

namespace WorldCupEngine
{
    public class Contestent
    {
        private int _points;
        private int _tournaments;
        private int _wins;
        private int _losses;
        
        public string Name { get; set; }
  
        public int Points { get => _points; }
        public int Tornaments { get => _tournaments; }
        public int 

        public Contestent()
        {
            _points = 0;
            _tournaments = 0;
            _wins = 0;
            _losses = 0;
        }



        public void AddPoints(int p) { _points += p; }

        public int Picked()
        {
            return Last32 + Last16 + Last8 + Last4 + Final + Win;
        }
    
    }
}
