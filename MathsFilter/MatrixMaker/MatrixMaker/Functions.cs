using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMaker
{
    public class Functions
    {
        private Dictionary<string, Func<double, double, double, double, double>> _funcmap;
        
        public Func<double,double,double,double,double> circle=(x,y,a,b)=>Math.Sqrt(a*x*x+b*y*y);
        public Func<double, double, double,double, double> tartan1 = (x, y, a,b) => Math.Sqrt(x * x + y * y) - x * y * Math.Cos(a * y);
        public Func<double, double, double, double,double> tartan2 = (x, y, a,b) => Math.Sqrt(x * x + y * y) - x * y * Math.Cos(a * x);
        public Func<double, double, double, double,double> tartan3 = (x, y, a,b) => Math.Sqrt(x * x + y * y) - x * y * Math.Sin(a * y);
        public Func<double, double, double, double,double> tartan4 = (x, y, a,b) => Math.Sqrt(x * x + y * y) - x * y * Math.Sin(a * x);
        public Func<double, double, double,double,double> onion = (x, y,a,b) => Math.Sqrt(a*x * x + b*y * y) * Math.Sqrt(Math.Abs(a*x + b*y));
        public Func<double,double,double, double, double> modernart = (x, y,a,b) => (Math.Pow((x + y), 4) + a * x +b * y) / (x * y);
        public Func<double, double, double, double, double> elo = (x, y, a, b) => (x + a) * (y + b) + Math.Pow(x + a, 2) + Math.Pow(y + b, 2);
        public Func<double, double, double, double, double> t2001 = (x, y, a, b) => (1 / (Math.Pow(x + y, a) + b));
        public Func<double, double, double, double, double> unionjack = (x, y, a, b) => (x * Math.Cos(a * y) + y * Math.Sin(b * x));
        public Func<double, double, double, double, double> jelly = (x, y, a, b) => (x * Math.Cos(a*y * Math.PI / 180)) + (y * Math.Sin(b*x * Math.PI / 180) + (x + y) * Math.Sin((x + y) * Math.PI / 180));

    
        public Functions()
        {
            _funcmap = new Dictionary<string, Func<double,double, double, double, double>>();
            _funcmap.Add("circle", circle);
            _funcmap.Add("tartan1", tartan1);
            _funcmap.Add("tartan2", tartan2);
            _funcmap.Add("tartan3", tartan3);
            _funcmap.Add("tartan4", tartan4);
            _funcmap.Add("onion", onion);
            _funcmap.Add("modernart", modernart);
            _funcmap.Add("elo", elo);
            _funcmap.Add("t2001", t2001);
            _funcmap.Add("uj", unionjack);
            _funcmap.Add("jelly", jelly);
        }

        public Func<double, double, double, double, double> GetFunction(string name)
        {
            Func<double, double, double, double, double> togo;
            if (_funcmap.ContainsKey(name))
            {
                togo = _funcmap[name];
            }
            else
            {
                togo = circle;
            }
            return togo;
        }

        public bool HasFunction(string name)
        {
            return _funcmap.ContainsKey(name);
        }



    }
}
