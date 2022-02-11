using System;

namespace WorldCupEngine
{
    public class Contestent
    {
        private int _points;
        
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Last32 { get; set; }
        public int Last16 { get; set; }
        public int Last8 { get; set; }
        public int Last4 { get; set; }
        public int Final { get; set; }
        public int Win { get; set; }
        public int Points { get => _points; }

        public Contestent()
        {
            _points = 0;
        }

        public void AddPoints(int p) { _points += p; }

        public int Picked()
        {
            return Last32 + Last16 + Last8 + Last4 + Final + Win;
        }
    
    }
}
