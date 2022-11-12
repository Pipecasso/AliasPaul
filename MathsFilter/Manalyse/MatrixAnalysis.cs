using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFilter;

namespace Manalyse
{
    public class MatrixAnalysis
    {
        public double Minumum { get; private set; }
        public double Maximum { get; private set; }

        public double Q1 { get; private set; }
        public double Median { get; private set; }
        public double Q3 { get; private set; }

        public double Mean { get; private set; }

        public int ValCount { get; private set; }

        public int Area { get; private set; }
    

        private int _Negative;
        private int _mono;
        private int _duo;
        private int _rgb;
        int _tooHigh;
        int _tooHigh2;
        public int Negative => _Negative;
        public int Mono => _mono;
        public int Dou => _duo;
        public int RGB => _rgb;

        public int OutOfRange => _tooHigh;
        public int OutOfRange2 => _tooHigh2;


        private TransformMatrix _tm;
        private const int _rgbMax = 256 * 256 * 256;

        public MatrixAnalysis(TransformMatrix tm)
        {
            _tm = tm;
            _Negative = 0;
            _mono = 0;
            _duo = 0;
            _rgb = 0;
            _tooHigh = 0;
            _tooHigh2 = 0;
            Maximum = double.MinValue;
            Minumum = double.MaxValue;
            double intmax = System.Convert.ToDouble(int.MaxValue);

            Area = tm.Area;

            double[] sortedArray = new double[Area];
            HashSet<double> hashmat = new HashSet<double>();

            int tick = 0;
            for (int i=0;i<tm.Dimension2;i++)
            {
                for (int j=0;j<tm.Dimension2;j++)
                {
                    double val = tm[i,j];
                    if (Math.Abs(val) > intmax)
                    {
                        _tooHigh2++;
                    }
                    if (!hashmat.Contains(val)) { hashmat.Add(val); }   
                    int ticktest = i * tm.Dimension2 + j;
                    sortedArray[tick] = val;
                    if (tick != ticktest)
                    {
                        int fu = 0;
                    }    
                    if (val < 0)
                    {
                        _Negative++;
                    }
                    else if (val>=_rgbMax)
                    {
                        _tooHigh++;
                    }    
                    else
                    {
                        if (val < 256)
                        {
                            _mono++;
                        }
                        if (val < 65536)
                        {
                            _duo++;
                        }
                        _rgb++;
                    }
                    if (val > Maximum)
                    {
                        Maximum=val;
                    }
                    if (val< Minumum)
                    {
                        Minumum =val;
                    }
                    tick++;
                }
            }

            Array.Sort(sortedArray);
            int q1 = Area / 4;
            int q2 = Area / 2;
            int q3 = q1 + q2;
            Q1 = sortedArray[q1];
            Median = sortedArray[q2];
            Q3 = sortedArray[q3];
            Mean = sortedArray.Average();
            ValCount = hashmat.Count;

        }

        
    
    }
}
